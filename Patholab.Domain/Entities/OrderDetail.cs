using System.Collections.Generic;

namespace Patholab.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        public int? TestId { get; set; }
        public virtual Test? Test { get; set; }

        public int? PackageId { get; set; }
        public virtual TestPackage? TestPackage { get; set; }

        public int Quantity { get; set; } = 1;
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public bool SampleRequired { get; set; } = true;
        public string Status { get; set; } = string.Empty;

        public virtual ICollection<SampleTest> SampleTests { get; set; } = new List<SampleTest>();
    }
}
