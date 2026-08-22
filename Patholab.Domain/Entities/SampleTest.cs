using System;
using System.Collections.Generic;

namespace Patholab.Domain.Entities
{
    public class SampleTest : BaseEntity
    {
        public int SampleId { get; set; }
        public virtual Sample Sample { get; set; } = null!;

        public int OrderDetailId { get; set; }
        public virtual OrderDetail OrderDetail { get; set; } = null!;

        public int TestId { get; set; }
        public virtual Test Test { get; set; } = null!;

        public string Status { get; set; } = string.Empty; // Pending, Processing, Completed, Cancelled
        public int? AssignedToId { get; set; }
        public virtual User? AssignedTo { get; set; }

        public DateTime? StartedOn { get; set; }
        public DateTime? CompletedOn { get; set; }

        public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
