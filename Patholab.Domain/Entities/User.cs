using System;
using System.Collections.Generic;

namespace Patholab.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginDate { get; set; }

        public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}
