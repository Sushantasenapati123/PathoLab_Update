using System;

namespace Patholab.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "System";
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public bool DeletedFlag { get; set; } = false;
    }
}
