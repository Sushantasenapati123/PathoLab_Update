using System.Collections.Generic;

namespace Patholab.Domain.Entities
{
    public class TestParameter : BaseEntity
    {
        public int TestId { get; set; }
        public virtual Test Test { get; set; } = null!;
        public string ParameterCode { get; set; } = string.Empty;
        public string ParameterName { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
        public string ResultType { get; set; } = string.Empty; // Numeric, Text, Range, Selection
        public string? Unit { get; set; }
        public decimal? MaleMin { get; set; }
        public decimal? MaleMax { get; set; }
        public decimal? FemaleMin { get; set; }
        public decimal? FemaleMax { get; set; }
        public decimal? ChildMin { get; set; }
        public decimal? ChildMax { get; set; }
        public string? DefaultReferenceRange { get; set; }
        public decimal? CriticalLow { get; set; }
        public decimal? CriticalHigh { get; set; }
        public bool IsRequired { get; set; } = true;
        public bool IsActive { get; set; } = true;

        public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
