using System;
using System.Collections.Generic;
using Patholab.Domain.Enums;

namespace Patholab.Domain.Entities
{
    public class Sample : BaseEntity
    {
        public string SampleNumber { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; } = null!;
        public int SampleTypeId { get; set; }
        public virtual SampleType SampleType { get; set; } = null!;
        public string Barcode { get; set; } = string.Empty;
        public DateTime? CollectionDateTime { get; set; }
        public int? CollectedById { get; set; }
        public virtual User? CollectedBy { get; set; }
        public DateTime? ReceivedDateTime { get; set; }
        public int? ReceivedById { get; set; }
        public virtual User? ReceivedBy { get; set; }
        public SampleStatus SampleStatus { get; set; } = SampleStatus.Pending;
        public string? RejectionReason { get; set; }
        public string? Remarks { get; set; }

        public virtual ICollection<SampleTest> SampleTests { get; set; } = new List<SampleTest>();
    }
}
