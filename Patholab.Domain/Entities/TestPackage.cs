using System.Collections.Generic;

namespace Patholab.Domain.Entities
{
    public class TestPackage : BaseEntity
    {
        public string PackageCode { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<PackageTest> PackageTests { get; set; } = new List<PackageTest>();
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
