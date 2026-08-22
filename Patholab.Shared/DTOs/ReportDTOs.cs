using System;
using System.Collections.Generic;
using Patholab.Domain.Enums;

namespace Patholab.Shared.DTOs
{
    public class ReportDto
    {
        public int Id { get; set; }
        public string ReportNumber { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public string PatientGender { get; set; } = string.Empty;
        public string PatientCode { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public ReportStatus ReportStatus { get; set; }
        public DateTime ReportDate { get; set; }
        public string? VerifiedByName { get; set; }
        public DateTime? VerifiedOn { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string? PdfFilePath { get; set; }
        public string? Remarks { get; set; }
        public int VersionNumber { get; set; }
        public List<ReportDetailDto> ReportDetails { get; set; } = new List<ReportDetailDto>();
    }

    public class ReportDetailDto
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int TestId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public string? Interpretation { get; set; }
        public string? Remarks { get; set; }
    }

    public class VerifyReportRequest
    {
        public int ReportId { get; set; }
        public string? Remarks { get; set; }
    }

    public class ReportVerificationResponse
    {
        public string ReportNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientCode { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string VerifiedBy { get; set; } = string.Empty;
        public DateTime? VerifiedOn { get; set; }
    }
}
