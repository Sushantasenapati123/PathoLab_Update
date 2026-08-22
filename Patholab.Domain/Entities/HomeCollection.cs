using System;
using Patholab.Domain.Enums;

namespace Patholab.Domain.Entities
{
    public class HomeCollection : BaseEntity
    {
        public string RequestNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; } = null!;
        public int? OrderId { get; set; }
        public virtual Order? Order { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public DateTime RequestedDate { get; set; }
        public string RequestedTime { get; set; } = string.Empty; // e.g. "08:00 AM - 10:00 AM"
        public int? AssignedAgentId { get; set; }
        public virtual User? AssignedAgent { get; set; }
        public HomeCollectionStatus Status { get; set; } = HomeCollectionStatus.Requested;
        public DateTime? CollectionDateTime { get; set; }
        public string? Remarks { get; set; }
    }
}
