using System;

namespace Patholab.Domain.Entities
{
    public class TestResultHistory
    {
        public int Id { get; set; }
        public int TestResultId { get; set; }
        public virtual TestResult TestResult { get; set; } = null!;

        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? OldStatus { get; set; }
        public string? NewStatus { get; set; }

        public int ChangedById { get; set; }
        public virtual User ChangedBy { get; set; } = null!;
        public DateTime ChangedOn { get; set; } = DateTime.UtcNow;
        public string Reason { get; set; } = string.Empty;
    }
}
