using System;
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
    public class BillingService : IBillingService
    {
        private readonly IApplicationDbContext _context;

        public BillingService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceDto> GetInvoiceByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var inv = await _context.Invoices
                .Include(x => x.Order)
                .Include(x => x.Patient)
                .Include(x => x.Payments)
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedFlag, cancellationToken);

            if (inv == null) throw new Exception("Invoice not found.");
            return MapToDto(inv);
        }

        public async Task<PagedResult<InvoiceDto>> GetInvoicesPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default)
        {
            var query = _context.Invoices
                .Include(x => x.Order)
                .Include(x => x.Patient)
                .Where(x => !x.DeletedFlag);

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                bool hasStatusMatch = Enum.TryParse<InvoiceStatus>(search, true, out var statusVal);
                query = query.Where(x => x.InvoiceNumber.ToLower().Contains(s) || 
                                         x.Patient.FirstName.ToLower().Contains(s) || 
                                         x.Patient.LastName.ToLower().Contains(s) ||
                                         (hasStatusMatch && x.InvoiceStatus == statusVal));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<InvoiceDto>
            {
                Items = items.Select(MapToDto).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        public async Task<PaymentDto> CollectPaymentAsync(CreatePaymentRequest request, string receivedBy, CancellationToken cancellationToken = default)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Order)
                .FirstOrDefaultAsync(i => i.Id == request.InvoiceId && !i.DeletedFlag, cancellationToken);

            if (invoice == null) throw new Exception("Invoice not found.");
            if (invoice.InvoiceStatus == InvoiceStatus.Paid) throw new Exception("Invoice is already fully paid.");

            // Strict Business Rule check: Payment Amount <= Invoice Due Amount
            if (request.Amount <= 0)
            {
                throw new Exception("Payment amount must be greater than zero.");
            }
            if (request.Amount > invoice.DueAmount)
            {
                throw new Exception($"Payment amount (₹{request.Amount}) cannot exceed the due amount (₹{invoice.DueAmount}).");
            }

            await _context.BeginTransactionAsync(cancellationToken);
            try
            {
                // Create Payment record
                var paymentCount = await _context.Payments.CountAsync(cancellationToken) + 1;
                var paymentNumber = $"PAY-2026-{paymentCount:D6}";

                var payment = new Payment
                {
                    InvoiceId = invoice.Id,
                    PaymentNumber = paymentNumber,
                    PaymentDate = DateTime.UtcNow,
                    Amount = request.Amount,
                    PaymentMode = request.PaymentMode,
                    TransactionReference = request.TransactionReference,
                    Remarks = request.Remarks,
                    ReceivedBy = receivedBy,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                _context.Payments.Add(payment);

                // Update Invoice
                invoice.PaidAmount += request.Amount;
                invoice.DueAmount -= request.Amount;
                invoice.InvoiceStatus = invoice.DueAmount == 0 ? InvoiceStatus.Paid : InvoiceStatus.PartiallyPaid;
                invoice.UpdatedOn = DateTime.UtcNow;
                invoice.UpdatedBy = "System";

                // Update Order
                invoice.Order.PaidAmount += request.Amount;
                invoice.Order.DueAmount -= request.Amount;
                invoice.Order.PaymentStatus = invoice.Order.DueAmount == 0 ? PaymentStatus.Paid : PaymentStatus.PartiallyPaid;
                invoice.Order.UpdatedOn = DateTime.UtcNow;
                invoice.Order.UpdatedBy = "System";

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync(cancellationToken);

                return new PaymentDto
                {
                    Id = payment.Id,
                    InvoiceId = payment.InvoiceId,
                    InvoiceNumber = invoice.InvoiceNumber,
                    PaymentNumber = payment.PaymentNumber,
                    PaymentDate = payment.PaymentDate,
                    Amount = payment.Amount,
                    PaymentMode = payment.PaymentMode,
                    TransactionReference = payment.TransactionReference,
                    Remarks = payment.Remarks,
                    ReceivedBy = payment.ReceivedBy
                };
            }
            catch
            {
                await _context.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task<PagedResult<PaymentDto>> GetPaymentsPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default)
        {
            var query = _context.Payments
                .Include(x => x.Invoice)
                .Where(x => !x.DeletedFlag);

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                query = query.Where(x => x.PaymentNumber.ToLower().Contains(s) || 
                                         x.Invoice.InvoiceNumber.ToLower().Contains(s) ||
                                         x.PaymentMode.ToLower().Contains(s));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var dtos = items.Select(p => new PaymentDto
            {
                Id = p.Id,
                InvoiceId = p.InvoiceId,
                InvoiceNumber = p.Invoice.InvoiceNumber,
                PaymentNumber = p.PaymentNumber,
                PaymentDate = p.PaymentDate,
                Amount = p.Amount,
                PaymentMode = p.PaymentMode,
                TransactionReference = p.TransactionReference,
                Remarks = p.Remarks,
                ReceivedBy = p.ReceivedBy
            }).ToList();

            return new PagedResult<PaymentDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        private static InvoiceDto MapToDto(Invoice i)
        {
            return new InvoiceDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                OrderId = i.OrderId,
                OrderNumber = i.Order?.OrderNumber ?? string.Empty,
                PatientId = i.PatientId,
                PatientName = i.Patient != null ? $"{i.Patient.FirstName} {i.Patient.LastName}" : string.Empty,
                PatientCode = i.Patient?.PatientCode ?? string.Empty,
                InvoiceDate = i.InvoiceDate,
                GrossAmount = i.GrossAmount,
                DiscountAmount = i.DiscountAmount,
                TaxAmount = i.TaxAmount,
                NetAmount = i.NetAmount,
                PaidAmount = i.PaidAmount,
                DueAmount = i.DueAmount,
                InvoiceStatus = i.InvoiceStatus,
                Payments = i.Payments?.Select(p => new PaymentDto
                {
                    Id = p.Id,
                    InvoiceId = p.InvoiceId,
                    InvoiceNumber = i.InvoiceNumber,
                    PaymentNumber = p.PaymentNumber,
                    PaymentDate = p.PaymentDate,
                    Amount = p.Amount,
                    PaymentMode = p.PaymentMode,
                    TransactionReference = p.TransactionReference,
                    Remarks = p.Remarks,
                    ReceivedBy = p.ReceivedBy
                }).ToList() ?? new List<PaymentDto>()
            };
        }
    }
}
