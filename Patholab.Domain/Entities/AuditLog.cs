using System;

namespace Patholab.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public virtual User? User { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // Login, Logout, Create, etc.
        public string TableName { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
