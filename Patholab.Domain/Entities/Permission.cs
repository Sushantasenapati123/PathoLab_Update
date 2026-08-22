using System.Collections.Generic;

namespace Patholab.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public string PermissionCode { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
