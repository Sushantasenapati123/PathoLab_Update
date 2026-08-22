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
    public class SampleService : ISampleService
    {
        private readonly IApplicationDbContext _context;

        public SampleService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SampleDto> GetSampleByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var s = await _context.Samples
                .Include(x => x.Order)
                .Include(x => x.Patient)
                .Include(x => x.SampleType)
                .Include(x => x.CollectedBy)
                .Include(x => x.ReceivedBy)
                .Include(x => x.SampleTests)
                .ThenInclude(st => st.Test)
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedFlag, cancellationToken);

            if (s == null) throw new Exception("Sample not found.");
            return MapToDto(s);
        }

        public async Task<PagedResult<SampleDto>> GetSamplesPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default)
        {
            var query = _context.Samples
                .Include(x => x.Order)
                .Include(x => x.Patient)
                .Include(x => x.SampleType)
                .Where(x => !x.DeletedFlag);

            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                bool hasStatusMatch = Enum.TryParse<SampleStatus>(search, true, out var statusVal);
                query = query.Where(x => x.SampleNumber.ToLower().Contains(searchLower) || 
                                         x.Barcode.ToLower().Contains(searchLower) || 
                                         x.Patient.FirstName.ToLower().Contains(searchLower) || 
                                         x.Patient.LastName.ToLower().Contains(searchLower) ||
                                         (hasStatusMatch && x.SampleStatus == statusVal));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<SampleDto>
            {
                Items = items.Select(MapToDto).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        public async Task<List<SampleDto>> GetPendingSamplesAsync(CancellationToken cancellationToken = default)
        {
            var items = await _context.Samples
                .Include(x => x.Order)
                .Include(x => x.Patient)
                .Include(x => x.SampleType)
                .Where(x => x.SampleStatus == SampleStatus.Pending && !x.DeletedFlag)
                .OrderByDescending(x => x.Id)
                .ToListAsync(cancellationToken);

            return items.Select(MapToDto).ToList();
        }

        public async Task CollectSampleAsync(int sampleId, string barcode, int collectedById, CancellationToken cancellationToken = default)
        {
            var s = await _context.Samples
                .Include(x => x.Order)
                .Include(x => x.SampleTests)
                .FirstOrDefaultAsync(x => x.Id == sampleId && !x.DeletedFlag, cancellationToken);

            if (s == null) throw new Exception("Sample not found.");
            if (s.SampleStatus != SampleStatus.Pending) throw new Exception("Sample is already collected or processed.");

            await _context.BeginTransactionAsync(cancellationToken);
            try
            {
                s.Barcode = barcode;
                s.SampleStatus = SampleStatus.Collected;
                s.CollectionDateTime = DateTime.UtcNow;
                s.CollectedById = collectedById;
                s.UpdatedOn = DateTime.UtcNow;
                s.UpdatedBy = "System";

                // Update mapping test statuses
                foreach (var st in s.SampleTests)
                {
                    st.Status = "Collected";
                    st.UpdatedOn = DateTime.UtcNow;
                    st.UpdatedBy = "System";
                }

                // Update Order Status to Processing
                s.Order.OrderStatus = OrderStatus.SampleCollected;
                s.Order.UpdatedOn = DateTime.UtcNow;
                s.Order.UpdatedBy = "System";

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _context.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task ReceiveSampleAsync(int sampleId, int receivedById, CancellationToken cancellationToken = default)
        {
            var s = await _context.Samples
                .Include(x => x.Order)
                .Include(x => x.SampleTests)
                .FirstOrDefaultAsync(x => x.Id == sampleId && !x.DeletedFlag, cancellationToken);

            if (s == null) throw new Exception("Sample not found.");
            if (s.SampleStatus != SampleStatus.Collected) throw new Exception("Sample must be collected before it can be received.");

            await _context.BeginTransactionAsync(cancellationToken);
            try
            {
                s.SampleStatus = SampleStatus.Received;
                s.ReceivedDateTime = DateTime.UtcNow;
                s.ReceivedById = receivedById;
                s.UpdatedOn = DateTime.UtcNow;
                s.UpdatedBy = "System";

                // Update mapping tests
                foreach (var st in s.SampleTests)
                {
                    st.Status = "Processing";
                    st.UpdatedOn = DateTime.UtcNow;
                    st.UpdatedBy = "System";

                    // Pre-generate pending result entries for test parameters!
                    // This makes result-entry UI automatically pull parameter fields.
                    var parameters = await _context.TestParameters
                        .Where(tp => tp.TestId == st.TestId && tp.IsActive && !tp.DeletedFlag)
                        .ToListAsync(cancellationToken);

                    foreach (var param in parameters)
                    {
                        var exists = await _context.TestResults.AnyAsync(r => r.SampleTestId == st.Id && r.TestParameterId == param.Id, cancellationToken);
                        if (!exists)
                        {
                            var result = new TestResult
                            {
                                SampleTestId = st.Id,
                                TestParameterId = param.Id,
                                ResultStatus = ResultStatus.Pending,
                                ReferenceRange = param.DefaultReferenceRange,
                                Unit = param.Unit,
                                CreatedOn = DateTime.UtcNow,
                                CreatedBy = "System",
                                RowVersion = new byte[8] // placeholder for row version
                            };
                            _context.TestResults.Add(result);
                        }
                    }
                }

                s.Order.OrderStatus = OrderStatus.Processing;
                s.Order.UpdatedOn = DateTime.UtcNow;
                s.Order.UpdatedBy = "System";

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _context.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task RejectSampleAsync(int sampleId, string reason, string? remarks, int rejectedById, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(reason)) throw new Exception("Rejection reason is mandatory.");

            var s = await _context.Samples
                .Include(x => x.Order)
                .Include(x => x.SampleTests)
                .FirstOrDefaultAsync(x => x.Id == sampleId && !x.DeletedFlag, cancellationToken);

            if (s == null) throw new Exception("Sample not found.");

            await _context.BeginTransactionAsync(cancellationToken);
            try
            {
                s.SampleStatus = SampleStatus.Rejected;
                s.RejectionReason = reason;
                s.Remarks = (s.Remarks + $"\n[Rejected: {remarks}]").Trim();
                s.UpdatedOn = DateTime.UtcNow;
                s.UpdatedBy = "System";

                foreach (var st in s.SampleTests)
                {
                    st.Status = "Cancelled";
                    st.UpdatedOn = DateTime.UtcNow;
                    st.UpdatedBy = "System";
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

        private static SampleDto MapToDto(Sample s)
        {
            return new SampleDto
            {
                Id = s.Id,
                SampleNumber = s.SampleNumber,
                OrderId = s.OrderId,
                OrderNumber = s.Order?.OrderNumber ?? string.Empty,
                PatientId = s.PatientId,
                PatientName = s.Patient != null ? $"{s.Patient.FirstName} {s.Patient.LastName}" : string.Empty,
                PatientCode = s.Patient?.PatientCode ?? string.Empty,
                SampleTypeId = s.SampleTypeId,
                SampleTypeName = s.SampleType?.SampleTypeName ?? string.Empty,
                Barcode = s.Barcode,
                CollectionDateTime = s.CollectionDateTime,
                CollectedByName = s.CollectedBy?.FullName,
                ReceivedDateTime = s.ReceivedDateTime,
                ReceivedByName = s.ReceivedBy?.FullName,
                SampleStatus = s.SampleStatus,
                RejectionReason = s.RejectionReason,
                Remarks = s.Remarks,
                SampleTests = s.SampleTests?.Select(st => new SampleTestDto
                {
                    Id = st.Id,
                    SampleId = st.SampleId,
                    SampleNumber = s.SampleNumber,
                    OrderDetailId = st.OrderDetailId,
                    TestId = st.TestId,
                    TestName = st.Test?.TestName ?? string.Empty,
                    Status = st.Status,
                    AssignedToName = st.AssignedTo?.FullName,
                    StartedOn = st.StartedOn,
                    CompletedOn = st.CompletedOn
                }).ToList() ?? new List<SampleTestDto>()
            };
        }
    }
}
