using System.Collections.Generic;

namespace Patholab.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string DepartmentCode { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
    }
}
