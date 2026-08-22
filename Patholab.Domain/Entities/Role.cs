using System.Collections.Generic;

namespace Patholab.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
