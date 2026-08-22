using System;
using System.Collections.Generic;
using Patholab.Domain.Enums;

namespace Patholab.Domain.Entities
{
    public class TestResult : BaseEntity
    {
        public int SampleTestId { get; set; }
        public virtual SampleTest SampleTest { get; set; } = null!;

        public int TestParameterId { get; set; }
        public virtual TestParameter TestParameter { get; set; } = null!;

        public string? ResultValue { get; set; }
        public decimal? ResultNumericValue { get; set; }
        public string? ResultText { get; set; }
        public ResultStatus ResultStatus { get; set; } = ResultStatus.Pending;
        public string? ReferenceRange { get; set; }
        public string? Unit { get; set; }
        public string? Remarks { get; set; }
        public bool IsCritical { get; set; } = false;

        public int? EnteredById { get; set; }
        public virtual User? EnteredBy { get; set; }
        public DateTime? EnteredOn { get; set; }

        public int? VerifiedById { get; set; }
        public virtual User? VerifiedBy { get; set; }
        public DateTime? VerifiedOn { get; set; }

        public byte[] RowVersion { get; set; } = null!;

        public virtual ICollection<TestResultHistory> TestResultHistories { get; set; } = new List<TestResultHistory>();
    }
}
