using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Patholab.Application.Interfaces;
using Patholab.Domain.Entities;
using Patholab.Domain.Enums;
using Patholab.Shared.DTOs;
using Patholab.Shared.Models;

namespace Patholab.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IApplicationDbContext _context;

        public OrderService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
        {
            await _context.BeginTransactionAsync(cancellationToken);
            try
            {
                // 1. Fetch Patient
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.Id == request.PatientId && !p.DeletedFlag, cancellationToken);
                if (patient == null) throw new Exception("Patient not found.");

                // 2. Validate referral Doctor if provided
                if (request.DoctorId.HasValue)
                {
                    var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == request.DoctorId && !d.DeletedFlag, cancellationToken);
                    if (!doctorExists) throw new Exception("Referral doctor not found.");
                }

                // 3. Authoritative Price Calculation from Database
                decimal grossAmount = 0;
                var orderDetailsList = new List<OrderDetail>();

                // Fetch Tests
                if (request.TestIds.Any())
                {
                    var tests = await _context.Tests
                        .Where(t => request.TestIds.Contains(t.Id) && t.IsActive && !t.DeletedFlag)
                        .ToListAsync(cancellationToken);

                    foreach (var test in tests)
                    {
                        var rate = test.Price;
                        if (request.CustomRates != null)
                        {
                            var customRate = request.CustomRates.FirstOrDefault(r => r.TestId == test.Id);
                            if (customRate != null)
                            {
                                rate = customRate.Rate;
                            }
                        }
                        grossAmount += rate;
                        orderDetailsList.Add(new OrderDetail
                        {
                            TestId = test.Id,
                            Rate = rate,
                            Amount = rate,
                            Quantity = 1,
                            SampleRequired = true,
                            Status = "Pending",
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = "System"
                        });
                    }
                }

                // Fetch Packages
                if (request.PackageIds.Any())
                {
                    var packages = await _context.TestPackages
                        .Include(p => p.PackageTests)
                        .ThenInclude(pt => pt.Test)
                        .Where(p => request.PackageIds.Contains(p.Id) && p.IsActive && !p.DeletedFlag)
                        .ToListAsync(cancellationToken);

                    foreach (var pack in packages)
                    {
                        var rate = pack.PackagePrice;
                        if (request.CustomRates != null)
                        {
                            var customRate = request.CustomRates.FirstOrDefault(r => r.PackageId == pack.Id);
                            if (customRate != null)
                            {
                                rate = customRate.Rate;
                            }
                        }
                        grossAmount += rate;
                        orderDetailsList.Add(new OrderDetail
                        {
                            PackageId = pack.Id,
                            Rate = rate,
                            Amount = rate,
                            Quantity = 1,
                            SampleRequired = true,
                            Status = "Pending",
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = "System"
                        });
                    }
                }

                if (!orderDetailsList.Any())
                {
                    throw new Exception("At least one valid test or package must be selected.");
                }

                // Calculate Net
                var discountAmount = request.DiscountAmount;
                if (discountAmount > grossAmount)
                {
                    throw new Exception("Discount cannot exceed order total amount.");
                }
                var netAmount = grossAmount - discountAmount;
                var paidAmount = Math.Min(request.PaidAmount, netAmount);
                var dueAmount = netAmount - paidAmount;

                // 4. Generate Order Number
                var orderCount = await _context.Orders.CountAsync(cancellationToken) + 1;
                var orderNumber = $"ORD-2026-{orderCount:D6}";

                // 5. Create Order
                var order = new Order
                {
                    OrderNumber = orderNumber,
                    PatientId = request.PatientId,
                    DoctorId = request.DoctorId,
                    OrderDate = DateTime.UtcNow,
                    OrderStatus = OrderStatus.Registered,
                    TotalAmount = grossAmount,
                    DiscountAmount = discountAmount,
                    NetAmount = netAmount,
                    PaidAmount = paidAmount,
                    DueAmount = dueAmount,
                    PaymentStatus = paidAmount == 0 ? PaymentStatus.Unpaid :
                                    dueAmount == 0 ? PaymentStatus.Paid : PaymentStatus.PartiallyPaid,
                    Remarks = request.Remarks,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                foreach (var detail in orderDetailsList)
                {
                    order.OrderDetails.Add(detail);
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync(cancellationToken);

                // 6. Generate Samples
                // We group by Sample Type ID across all test details to ensure we only request one specimen collection per type!
                var sampleTypeIdsNeeded = new HashSet<int>();

                // Get sample types from individual tests
                if (request.TestIds.Any())
                {
                    var tests = await _context.Tests
                        .Where(t => request.TestIds.Contains(t.Id))
                        .Select(t => t.SampleTypeId)
                        .ToListAsync(cancellationToken);
                    foreach (var sId in tests) sampleTypeIdsNeeded.Add(sId);
                }

                // Get sample types from package tests
                if (request.PackageIds.Any())
                {
                    var pkgSampleTypes = await _context.PackageTests
                        .Include(pt => pt.Test)
                        .Where(pt => request.PackageIds.Contains(pt.PackageId))
                        .Select(pt => pt.Test.SampleTypeId)
                        .ToListAsync(cancellationToken);
                    foreach (var sId in pkgSampleTypes) sampleTypeIdsNeeded.Add(sId);
                }

                var samplesList = new List<Sample>();
                var sampleIndex = await _context.Samples.CountAsync(cancellationToken) + 1;

                foreach (var stId in sampleTypeIdsNeeded)
                {
                    var sampleNum = $"SMP-2026-{sampleIndex:D6}";
                    var barcode = $"BAR-2026-{sampleIndex:D6}";
                    sampleIndex++;

                    var sample = new Sample
                    {
                        SampleNumber = sampleNum,
                        OrderId = order.Id,
                        PatientId = request.PatientId,
                        SampleTypeId = stId,
                        Barcode = barcode,
                        SampleStatus = SampleStatus.Pending,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = "System"
                    };

                    _context.Samples.Add(sample);
                    samplesList.Add(sample);
                }
                await _context.SaveChangesAsync(cancellationToken);

                // Map OrderDetails to SampleTests
                foreach (var detail in order.OrderDetails)
                {
                    var testIdsInDetail = new List<int>();
                    if (detail.TestId.HasValue)
                    {
                        testIdsInDetail.Add(detail.TestId.Value);
                    }
                    else if (detail.PackageId.HasValue)
                    {
                        var pkgTests = await _context.PackageTests
                            .Where(pt => pt.PackageId == detail.PackageId.Value)
                            .Select(pt => pt.TestId)
                            .ToListAsync(cancellationToken);
                        testIdsInDetail.AddRange(pkgTests);
                    }

                    foreach (var tId in testIdsInDetail)
                    {
                        var testEntity = await _context.Tests.FindAsync(new object[] { tId }, cancellationToken);
                        if (testEntity == null) continue;

                        var matchingSample = samplesList.FirstOrDefault(s => s.SampleTypeId == testEntity.SampleTypeId);
                        if (matchingSample != null)
                        {
                            var sampleTest = new SampleTest
                            {
                                SampleId = matchingSample.Id,
                                OrderDetailId = detail.Id,
                                TestId = tId,
                                Status = "Pending",
                                CreatedOn = DateTime.UtcNow,
                                CreatedBy = "System"
                            };
                            _context.SampleTests.Add(sampleTest);
                        }
                    }
                }
                await _context.SaveChangesAsync(cancellationToken);

                // Update OrderStatus to Registered
                order.OrderStatus = OrderStatus.SamplePending;
                await _context.SaveChangesAsync(cancellationToken);

                // 7. Create Invoice
                var invoiceCount = await _context.Invoices.CountAsync(cancellationToken) + 1;
                var invoiceNum = $"INV-2026-{invoiceCount:D6}";

                var invoice = new Invoice
                {
                    InvoiceNumber = invoiceNum,
                    OrderId = order.Id,
                    PatientId = request.PatientId,
                    InvoiceDate = DateTime.UtcNow,
                    GrossAmount = grossAmount,
                    DiscountAmount = discountAmount,
                    TaxAmount = 0.00m,
                    NetAmount = netAmount,
                    PaidAmount = paidAmount,
                    DueAmount = dueAmount,
                    InvoiceStatus = paidAmount == 0 ? InvoiceStatus.Unpaid :
                                    dueAmount == 0 ? InvoiceStatus.Paid : InvoiceStatus.PartiallyPaid,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync(cancellationToken);

                // 8. Create Payment record if PaidAmount > 0
                if (paidAmount > 0)
                {
                    var paymentCount = await _context.Payments.CountAsync(cancellationToken) + 1;
                    var paymentNum = $"PAY-2026-{paymentCount:D6}";

                    var payment = new Payment
                    {
                        InvoiceId = invoice.Id,
                        PaymentNumber = paymentNum,
                        PaymentDate = DateTime.UtcNow,
                        Amount = paidAmount,
                        PaymentMode = string.IsNullOrEmpty(request.PaymentMode) ? "Cash" : request.PaymentMode,
                        TransactionReference = request.TransactionReference,
                        ReceivedBy = "System",
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = "System"
                    };

                    _context.Payments.Add(payment);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                await _context.CommitTransactionAsync(cancellationToken);

                return await GetOrderByIdAsync(order.Id, cancellationToken);
            }
            catch (Exception ex)
            {
                await _context.RollbackTransactionAsync(cancellationToken);
                throw new Exception($"Failed to place order: {ex.Message}", ex);
            }
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders
                .Include(o => o.Patient)
                .Include(o => o.Doctor)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Test)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.TestPackage)
                .Include(o => o.Samples)
                .ThenInclude(s => s.SampleType)
                .FirstOrDefaultAsync(o => o.Id == id && !o.DeletedFlag, cancellationToken);

            if (order == null) throw new Exception("Order not found.");
            return MapToDto(order);
        }

        public async Task<OrderDto> GetOrderByNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders
                .Include(o => o.Patient)
                .Include(o => o.Doctor)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Test)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.TestPackage)
                .Include(o => o.Samples)
                .ThenInclude(s => s.SampleType)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber && !o.DeletedFlag, cancellationToken);

            if (order == null) throw new Exception("Order not found.");
            return MapToDto(order);
        }

        public async Task<PagedResult<OrderDto>> GetOrdersPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default)
        {
            var query = _context.Orders
                .Include(o => o.Patient)
                .Include(o => o.Doctor)
                .Where(o => !o.DeletedFlag);

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                bool hasStatusMatch = Enum.TryParse<OrderStatus>(search, true, out var statusVal);
                query = query.Where(o => o.OrderNumber.ToLower().Contains(s) || 
                                         o.Patient.FirstName.ToLower().Contains(s) || 
                                         o.Patient.LastName.ToLower().Contains(s) ||
                                         (hasStatusMatch && o.OrderStatus == statusVal));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var items = await query
                .OrderByDescending(o => o.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var dtos = items.Select(MapToDto).ToList();

            return new PagedResult<OrderDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        public async Task CancelOrderAsync(int orderId, string remarks, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders
                .Include(o => o.Samples)
                .Include(o => o.Invoices)
                .FirstOrDefaultAsync(o => o.Id == orderId && !o.DeletedFlag, cancellationToken);

            if (order == null) throw new Exception("Order not found.");
            if (order.OrderStatus == OrderStatus.Verified || order.OrderStatus == OrderStatus.Published)
            {
                throw new Exception("Verified or Published orders cannot be cancelled.");
            }

            await _context.BeginTransactionAsync(cancellationToken);
            try
            {
                order.OrderStatus = OrderStatus.Cancelled;
                order.Remarks = (order.Remarks + $"\n[Cancelled: {remarks}]").Trim();
                order.UpdatedOn = DateTime.UtcNow;
                order.UpdatedBy = "System";

                // Cancel Samples
                foreach (var sample in order.Samples)
                {
                    sample.SampleStatus = SampleStatus.Cancelled;
                    sample.UpdatedOn = DateTime.UtcNow;
                    sample.UpdatedBy = "System";
                }

                // Cancel Invoice
                foreach (var invoice in order.Invoices)
                {
                    invoice.InvoiceStatus = InvoiceStatus.Cancelled;
                    invoice.UpdatedOn = DateTime.UtcNow;
                    invoice.UpdatedBy = "System";
                }

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _context.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        private static OrderDto MapToDto(Order o)
        {
            return new OrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                PatientId = o.PatientId,
                PatientName = $"{o.Patient.FirstName} {o.Patient.LastName}",
                PatientCode = o.Patient.PatientCode,
                PatientMobile = o.Patient.Mobile,
                DoctorId = o.DoctorId,
                DoctorName = o.Doctor?.DoctorName ?? "Self",
                OrderDate = o.OrderDate,
                OrderStatus = o.OrderStatus,
                TotalAmount = o.TotalAmount,
                DiscountAmount = o.DiscountAmount,
                NetAmount = o.NetAmount,
                PaidAmount = o.PaidAmount,
                DueAmount = o.DueAmount,
                PaymentStatus = o.PaymentStatus,
                Remarks = o.Remarks,
                OrderDetails = o.OrderDetails?.Select(d => new OrderDetailDto
                {
                    Id = d.Id,
                    OrderId = d.OrderId,
                    TestId = d.TestId,
                    TestName = d.Test?.TestName ?? string.Empty,
                    TestCode = d.Test?.TestCode ?? string.Empty,
                    PackageId = d.PackageId,
                    PackageName = d.TestPackage?.PackageName ?? string.Empty,
                    PackageCode = d.TestPackage?.PackageCode ?? string.Empty,
                    Quantity = d.Quantity,
                    Rate = d.Rate,
                    Discount = d.Discount,
                    Amount = d.Amount,
                    SampleRequired = d.SampleRequired,
                    Status = d.Status
                }).ToList() ?? new List<OrderDetailDto>(),
                Samples = o.Samples?.Select(s => new SampleDto
                {
                    Id = s.Id,
                    SampleNumber = s.SampleNumber,
                    SampleTypeName = s.SampleType?.SampleTypeName ?? string.Empty,
                    Barcode = s.Barcode,
                    SampleStatus = s.SampleStatus
                }).ToList() ?? new List<SampleDto>()
            };
        }
    }
}
