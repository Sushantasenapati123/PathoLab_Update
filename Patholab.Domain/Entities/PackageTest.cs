namespace Patholab.Domain.Entities
{
    public class PackageTest : BaseEntity
    {
        public int PackageId { get; set; }
        public virtual TestPackage TestPackage { get; set; } = null!;

        public int TestId { get; set; }
        public virtual Test Test { get; set; } = null!;
    }
}
