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

namespace Patholab.Application.Services
{
    public class TestResultService : ITestResultService
    {
        private readonly IApplicationDbContext _context;

        public TestResultService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task EnterResultsAsync(EnterResultsRequest request, int enteredById, CancellationToken cancellationToken = default)
        {
            var sample = await _context.Samples
                .Include(s => s.Order)
                .Include(s => s.Patient)
                .FirstOrDefaultAsync(s => s.Id == request.SampleId && !s.DeletedFlag, cancellationToken);

            if (sample == null) throw new Exception("Sample not found.");

            await _context.BeginTransactionAsync(cancellationToken);
            try
            {
                foreach (var rItem in request.Results)
                {
                    // Fetch existing result record or create one
                    var result = await _context.TestResults
                        .Include(r => r.TestParameter)
                        .FirstOrDefaultAsync(r => r.SampleTest.SampleId == request.SampleId && 
                                                 r.TestParameterId == rItem.TestParameterId, cancellationToken);

                    if (result == null)
                    {
                        // Create result record if not found (fallback, though pre-generated in ReceiveSample)
                        var sampleTest = await _context.SampleTests
                            .FirstOrDefaultAsync(st => st.SampleId == request.SampleId && st.Test.TestParameters.Any(tp => tp.Id == rItem.TestParameterId), cancellationToken);

                        if (sampleTest == null) continue;

                        result = new TestResult
                        {
                            SampleTestId = sampleTest.Id,
                            TestParameterId = rItem.TestParameterId,
                            ResultStatus = ResultStatus.Pending,
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = "System",
                            RowVersion = new byte[8]
                        };
                        _context.TestResults.Add(result);
                        await _context.SaveChangesAsync(cancellationToken);
                    }

                    // Handle Clinical Audit Log if value has changed
                    bool isModified = !string.IsNullOrEmpty(result.ResultValue) && result.ResultValue != rItem.ResultValue;
                    string oldValue = result.ResultValue ?? string.Empty;
                    ResultStatus oldStatus = result.ResultStatus;

                    result.ResultValue = rItem.ResultValue;
                    result.Remarks = rItem.Remarks;
                    result.EnteredById = enteredById;
                    result.EnteredOn = DateTime.UtcNow;
                    result.UpdatedOn = DateTime.UtcNow;
                    result.UpdatedBy = "System";

                    // Parse numeric value if possible for range checks
                    if (decimal.TryParse(rItem.ResultValue, out var numericValue))
                    {
                        result.ResultNumericValue = numericValue;
                        // Auto calculate status
                        CalculateResultStatus(result, sample.Patient, numericValue);
                    }
                    else
                    {
                        result.ResultNumericValue = null;
                        result.ResultStatus = string.IsNullOrEmpty(rItem.ResultValue) ? ResultStatus.Pending : ResultStatus.Normal;
                    }

                    if (isModified)
                    {
                        var history = new TestResultHistory
                        {
                            TestResultId = result.Id,
                            OldValue = oldValue,
                            NewValue = result.ResultValue,
                            OldStatus = oldStatus.ToString(),
                            NewStatus = result.ResultStatus.ToString(),
                            ChangedById = enteredById,
                            ChangedOn = DateTime.UtcNow,
                            Reason = "Clinical result correction/update"
                        };
                        _context.TestResultHistories.Add(history);
                    }
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

        public async Task<List<TestResultDto>> GetResultsBySampleIdAsync(int sampleId, CancellationToken cancellationToken = default)
        {
            var results = await _context.TestResults
                .Include(r => r.TestParameter)
                .Include(r => r.EnteredBy)
                .Include(r => r.VerifiedBy)
                .Where(r => r.SampleTest.SampleId == sampleId)
                .OrderBy(r => r.TestParameter.DisplayOrder)
                .ToListAsync(cancellationToken);

            return results.Select(r => new TestResultDto
            {
                Id = r.Id,
                SampleTestId = r.SampleTestId,
                TestParameterId = r.TestParameterId,
                ParameterName = r.TestParameter.ParameterName,
                ParameterCode = r.TestParameter.ParameterCode,
                DisplayOrder = r.TestParameter.DisplayOrder,
                ResultValue = r.ResultValue,
                ResultNumericValue = r.ResultNumericValue,
                ResultText = r.ResultText,
                ResultStatus = r.ResultStatus,
                ReferenceRange = r.ReferenceRange ?? r.TestParameter.DefaultReferenceRange,
                Unit = r.Unit ?? r.TestParameter.Unit,
                Remarks = r.Remarks,
                IsCritical = r.IsCritical,
                EnteredByName = r.EnteredBy?.FullName,
                EnteredOn = r.EnteredOn,
                VerifiedByName = r.VerifiedBy?.FullName,
                VerifiedOn = r.VerifiedOn
            }).ToList();
        }

        public async Task SubmitResultsForVerificationAsync(int sampleId, CancellationToken cancellationToken = default)
        {
            var sample = await _context.Samples
                .Include(s => s.Order)
                .Include(s => s.SampleTests)
                .FirstOrDefaultAsync(s => s.Id == sampleId && !s.DeletedFlag, cancellationToken);

            if (sample == null) throw new Exception("Sample not found.");

            // Check if any results are still pending
            var pendingResults = await _context.TestResults
                .AnyAsync(r => r.SampleTest.SampleId == sampleId && r.ResultStatus == ResultStatus.Pending, cancellationToken);

            if (pendingResults)
            {
                throw new Exception("Cannot submit. Some test parameter results are still pending entry.");
            }

            await _context.BeginTransactionAsync(cancellationToken);
            try
            {
                sample.SampleStatus = SampleStatus.Completed;
                sample.UpdatedOn = DateTime.UtcNow;
                sample.UpdatedBy = "System";

                foreach (var st in sample.SampleTests)
                {
                    st.Status = "Completed";
                    st.CompletedOn = DateTime.UtcNow;
                    st.UpdatedOn = DateTime.UtcNow;
                    st.UpdatedBy = "System";
                }

                // Check if ALL samples under the order are completed to set order status to UnderVerification
                var allSamplesCompleted = await _context.Samples
                    .Where(s => s.OrderId == sample.OrderId && !s.DeletedFlag)
                    .AllAsync(s => s.SampleStatus == SampleStatus.Completed || s.SampleStatus == SampleStatus.Rejected || s.SampleStatus == SampleStatus.Cancelled, cancellationToken);

                if (allSamplesCompleted)
                {
                    sample.Order.OrderStatus = OrderStatus.ResultEntered;
                    // Proactively move to UnderVerification for Pathologist verification
                    sample.Order.OrderStatus = OrderStatus.UnderVerification;
                    sample.Order.UpdatedOn = DateTime.UtcNow;
                    sample.Order.UpdatedBy = "System";

                    // Pre-create Report shell if not exists
                    var reportExists = await _context.Reports.AnyAsync(rep => rep.OrderId == sample.OrderId, cancellationToken);
                    if (!reportExists)
                    {
                        var reportCount = await _context.Reports.CountAsync(cancellationToken) + 1;
                        var reportNum = $"REP-2026-{reportCount:D6}";

                        var report = new Report
                        {
                            ReportNumber = reportNum,
                            OrderId = sample.OrderId,
                            PatientId = sample.PatientId,
                            ReportStatus = ReportStatus.Draft,
                            ReportDate = DateTime.UtcNow,
                            VersionNumber = 1,
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = "System"
                        };

                        _context.Reports.Add(report);
                    }
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

        public async Task<List<TestResultHistoryDto>> GetHistoryAsync(int resultId, CancellationToken cancellationToken = default)
        {
            var history = await _context.TestResultHistories
                .Include(h => h.TestResult)
                .ThenInclude(r => r.TestParameter)
                .Include(h => h.ChangedBy)
                .Where(h => h.TestResultId == resultId)
                .OrderByDescending(h => h.ChangedOn)
                .ToListAsync(cancellationToken);

            return history.Select(h => new TestResultHistoryDto
            {
                Id = h.Id,
                TestResultId = h.TestResultId,
                ParameterName = h.TestResult.TestParameter.ParameterName,
                OldValue = h.OldValue,
                NewValue = h.NewValue,
                OldStatus = h.OldStatus,
                NewStatus = h.NewStatus,
                ChangedByName = h.ChangedBy.FullName,
                ChangedOn = h.ChangedOn,
                Reason = h.Reason
            }).ToList();
        }

        private static void CalculateResultStatus(TestResult result, Patient patient, decimal value)
        {
            var p = result.TestParameter;
            var gender = patient.Gender.ToLower();
            var age = patient.Age;

            decimal? minRange = null;
            decimal? maxRange = null;

            // Choose range based on patient attributes
            if (age <= 12)
            {
                minRange = p.ChildMin;
                maxRange = p.ChildMax;
            }
            else if (gender.StartsWith("m"))
            {
                minRange = p.MaleMin;
                maxRange = p.MaleMax;
            }
            else
            {
                minRange = p.FemaleMin;
                maxRange = p.FemaleMax;
            }

            // Fallbacks
            if (!minRange.HasValue || !maxRange.HasValue)
            {
                // Fallback to defaults
                minRange = minRange ?? 0;
                maxRange = maxRange ?? 99999;
            }

            // Check Critical Thresholds
            if (p.CriticalLow.HasValue && value <= p.CriticalLow.Value)
            {
                result.ResultStatus = ResultStatus.Critical;
                result.IsCritical = true;
                return;
            }
            if (p.CriticalHigh.HasValue && value >= p.CriticalHigh.Value)
            {
                result.ResultStatus = ResultStatus.Critical;
                result.IsCritical = true;
                return;
            }

            // Check Normal Ranges
            result.IsCritical = false;
            if (value < minRange.Value)
            {
                result.ResultStatus = ResultStatus.Low;
            }
            else if (value > maxRange.Value)
            {
                result.ResultStatus = ResultStatus.High;
            }
            else
            {
                result.ResultStatus = ResultStatus.Normal;
            }
        }
    }
}
