namespace Patholab.Domain.Entities
{
    public class ReportDetail : BaseEntity
    {
        public int ReportId { get; set; }
        public virtual Report Report { get; set; } = null!;

        public int TestId { get; set; }
        public virtual Test Test { get; set; } = null!;

        public string TestName { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
        public string? Interpretation { get; set; }
        public string? Remarks { get; set; }
    }
}
