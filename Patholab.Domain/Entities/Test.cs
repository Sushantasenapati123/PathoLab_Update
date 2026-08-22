using System.Collections.Generic;

namespace Patholab.Domain.Entities
{
    public class Test : BaseEntity
    {
        public string TestCode { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; } = null!;
        public int SampleTypeId { get; set; }
        public virtual SampleType SampleType { get; set; } = null!;
        public string TestType { get; set; } = string.Empty; // Single, Profile
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int TATMinutes { get; set; } = 180;
        public bool IsActive { get; set; } = true;

        public virtual ICollection<TestParameter> TestParameters { get; set; } = new List<TestParameter>();
        public virtual ICollection<PackageTest> PackageTests { get; set; } = new List<PackageTest>();
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual ICollection<SampleTest> SampleTests { get; set; } = new List<SampleTest>();
        public virtual ICollection<ReportDetail> ReportDetails { get; set; } = new List<ReportDetail>();
    }
}
