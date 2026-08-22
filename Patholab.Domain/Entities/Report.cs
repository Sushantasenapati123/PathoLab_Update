using System;
using System.Collections.Generic;
using Patholab.Domain.Enums;

namespace Patholab.Domain.Entities
{
    public class Report : BaseEntity
    {
        public string ReportNumber { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; } = null!;
        public ReportStatus ReportStatus { get; set; } = ReportStatus.Draft;
        public DateTime ReportDate { get; set; }
        public int? VerifiedById { get; set; }
        public virtual User? VerifiedBy { get; set; }
        public DateTime? VerifiedOn { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string? PdfFilePath { get; set; }
        public string? Remarks { get; set; }
        public int VersionNumber { get; set; } = 1;

        public virtual ICollection<ReportDetail> ReportDetails { get; set; } = new List<ReportDetail>();
    }
}
