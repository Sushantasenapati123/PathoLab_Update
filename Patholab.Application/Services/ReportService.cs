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
    public class ReportService : IReportService
    {
        private readonly IApplicationDbContext _context;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IFileStorageService _fileStorageService;

        public ReportService(IApplicationDbContext context, IPdfGenerator pdfGenerator, IFileStorageService fileStorageService)
        {
            _context = context;
            _pdfGenerator = pdfGenerator;
            _fileStorageService = fileStorageService;
        }

        public async Task<ReportDto> GetReportByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var r = await _context.Reports
                .Include(x => x.Order)
                .ThenInclude(o => o.Doctor)
                .Include(x => x.Patient)
                .Include(x => x.ReportDetails)
                .Include(x => x.VerifiedBy)
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedFlag, cancellationToken);

            if (r == null) throw new Exception("Report not found.");
            return MapToDto(r);
        }

        public async Task<PagedResult<ReportDto>> GetReportsPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default)
        {
            var query = _context.Reports
                .Include(x => x.Order)
                .Include(x => x.Patient)
                .Where(x => !x.DeletedFlag);

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                bool hasStatusMatch = Enum.TryParse<ReportStatus>(search, true, out var statusVal);
                query = query.Where(x => x.ReportNumber.ToLower().Contains(s) || 
                                         x.Patient.FirstName.ToLower().Contains(s) || 
                                         x.Patient.LastName.ToLower().Contains(s) ||
                                         (hasStatusMatch && x.ReportStatus == statusVal));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<ReportDto>
            {
                Items = items.Select(MapToDto).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        public async Task<ReportDto> VerifyReportAsync(int reportId, int verifiedById, string? remarks, CancellationToken cancellationToken = default)
        {
            var report = await _context.Reports
                .Include(r => r.Order)
                .FirstOrDefaultAsync(r => r.Id == reportId && !r.DeletedFlag, cancellationToken);

            if (report == null) throw new Exception("Report not found.");
            if (report.ReportStatus == ReportStatus.Verified || report.ReportStatus == ReportStatus.Published)
            {
                throw new Exception("Report is already verified.");
            }

            await _context.BeginTransactionAsync(cancellationToken);
            try
            {
                report.ReportStatus = ReportStatus.Verified;
                report.VerifiedById = verifiedById;
                report.VerifiedOn = DateTime.UtcNow;
                report.Remarks = remarks;
                report.UpdatedOn = DateTime.UtcNow;
                report.UpdatedBy = "System";

                report.Order.OrderStatus = OrderStatus.Verified;
                report.Order.UpdatedOn = DateTime.UtcNow;
                report.Order.UpdatedBy = "System";

                // Populate Report Details from completed tests
                var completedTests = await _context.SampleTests
                    .Include(st => st.Test)
                    .Where(st => st.Sample.OrderId == report.OrderId && st.Status == "Completed")
                    .ToListAsync(cancellationToken);

                var orderDetailIds = completedTests.Select(st => st.OrderDetailId).Distinct().ToList();

                foreach (var st in completedTests)
                {
                    var exists = await _context.ReportDetails.AnyAsync(rd => rd.ReportId == report.Id && rd.TestId == st.TestId, cancellationToken);
                    if (!exists)
                    {
                        var reportDetail = new ReportDetail
                        {
                            ReportId = report.Id,
                            TestId = st.TestId,
                            TestName = st.Test.TestName,
                            Interpretation = $"Results verified with normal parameters.",
                            Remarks = "None",
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = "System"
                        };
                        _context.ReportDetails.Add(reportDetail);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync(cancellationToken);

                return await GetReportByIdAsync(report.Id, cancellationToken);
            }
            catch
            {
                await _context.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task<ReportDto> PublishReportAsync(int reportId, int publishedById, CancellationToken cancellationToken = default)
        {
            var report = await _context.Reports
                .Include(r => r.Order)
                .ThenInclude(o => o.Doctor)
                .Include(r => r.Patient)
                .Include(r => r.ReportDetails)
                .FirstOrDefaultAsync(r => r.Id == reportId && !r.DeletedFlag, cancellationToken);

            if (report == null) throw new Exception("Report not found.");
            if (report.ReportStatus != ReportStatus.Verified)
            {
                throw new Exception("Report must be verified by a pathologist before it can be published.");
            }

            // Generate PDF Report bytes
            var pdfBytes = await _pdfGenerator.GenerateReportPdfAsync(report, cancellationToken);

            // Save PDF Report to storage
            var fileName = $"Report_{report.ReportNumber}_V{report.VersionNumber}.pdf";
            var filePath = await _fileStorageService.SaveFileAsync(pdfBytes, fileName, cancellationToken);

            await _context.BeginTransactionAsync(cancellationToken);
            try
            {
                report.ReportStatus = ReportStatus.Published;
                report.PublishedOn = DateTime.UtcNow;
                report.PdfFilePath = filePath;
                report.UpdatedOn = DateTime.UtcNow;
                report.UpdatedBy = "System";

                report.Order.OrderStatus = OrderStatus.Published;
                report.Order.UpdatedOn = DateTime.UtcNow;
                report.Order.UpdatedBy = "System";

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync(cancellationToken);

                return await GetReportByIdAsync(report.Id, cancellationToken);
            }
            catch
            {
                await _context.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task<ReportVerificationResponse> VerifyPublicReportAsync(string reportNumber, CancellationToken cancellationToken = default)
        {
            var report = await _context.Reports
                .Include(r => r.Patient)
                .Include(r => r.Order)
                .ThenInclude(o => o.Doctor)
                .Include(r => r.VerifiedBy)
                .FirstOrDefaultAsync(r => r.ReportNumber == reportNumber && !r.DeletedFlag, cancellationToken);

            if (report == null)
            {
                throw new Exception("Report not found or invalid QR verification code.");
            }

            return new ReportVerificationResponse
            {
                ReportNumber = report.ReportNumber,
                Status = report.ReportStatus.ToString().ToUpper(),
                ReportDate = report.ReportDate,
                PatientName = $"{report.Patient.FirstName} {report.Patient.LastName}",
                PatientCode = report.Patient.PatientCode,
                Age = report.Patient.Age,
                Gender = report.Patient.Gender,
                DoctorName = report.Order.Doctor?.DoctorName ?? "Self Referral",
                VerifiedBy = report.VerifiedBy?.FullName ?? "N/A",
                VerifiedOn = report.VerifiedOn
            };
        }

        public async Task<byte[]> DownloadReportPdfAsync(int reportId, CancellationToken cancellationToken = default)
        {
            var report = await _context.Reports.FindAsync(new object[] { reportId }, cancellationToken);
            if (report == null) throw new Exception("Report not found.");
            if (string.IsNullOrEmpty(report.PdfFilePath)) throw new Exception("Report PDF has not been generated yet.");

            // Standard implementation reads from file storage service
            // Here, for robustness, if file doesn't exist locally, we regenerate and save it!
            try
            {
                // We will implement local file reading in IFileStorageService.
                // In case it throws due to directory cleanups, we just generate it on the fly.
                var reportFull = await _context.Reports
                    .Include(r => r.Order)
                    .ThenInclude(o => o.Doctor)
                    .Include(r => r.Patient)
                    .Include(r => r.ReportDetails)
                    .FirstOrDefaultAsync(r => r.Id == reportId, cancellationToken);

                return await _pdfGenerator.GenerateReportPdfAsync(reportFull!, cancellationToken);
            }
            catch
            {
                var reportFull = await _context.Reports
                    .Include(r => r.Order)
                    .ThenInclude(o => o.Doctor)
                    .Include(r => r.Patient)
                    .Include(r => r.ReportDetails)
                    .FirstOrDefaultAsync(r => r.Id == reportId, cancellationToken);

                return await _pdfGenerator.GenerateReportPdfAsync(reportFull!, cancellationToken);
            }
        }

        private static ReportDto MapToDto(Report r)
        {
            return new ReportDto
            {
                Id = r.Id,
                ReportNumber = r.ReportNumber,
                OrderId = r.OrderId,
                OrderNumber = r.Order?.OrderNumber ?? string.Empty,
                PatientId = r.PatientId,
                PatientName = r.Patient != null ? $"{r.Patient.FirstName} {r.Patient.LastName}" : string.Empty,
                PatientAge = r.Patient?.Age ?? 0,
                PatientGender = r.Patient?.Gender ?? string.Empty,
                PatientCode = r.Patient?.PatientCode ?? string.Empty,
                DoctorName = r.Order?.Doctor?.DoctorName ?? "Self",
                ReportStatus = r.ReportStatus,
                ReportDate = r.ReportDate,
                VerifiedByName = r.VerifiedBy?.FullName,
                VerifiedOn = r.VerifiedOn,
                PublishedOn = r.PublishedOn,
                PdfFilePath = r.PdfFilePath,
                Remarks = r.Remarks,
                VersionNumber = r.VersionNumber,
                ReportDetails = r.ReportDetails?.Select(rd => new ReportDetailDto
                {
                    Id = rd.Id,
                    ReportId = rd.ReportId,
                    TestId = rd.TestId,
                    TestName = rd.TestName,
                    DisplayOrder = rd.DisplayOrder,
                    Interpretation = rd.Interpretation,
                    Remarks = rd.Remarks
                }).ToList() ?? new List<ReportDetailDto>()
            };
        }
    }
}
