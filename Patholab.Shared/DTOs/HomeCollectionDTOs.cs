using System;
using Patholab.Domain.Enums;

namespace Patholab.Shared.DTOs
{
    public class HomeCollectionDto
    {
        public int Id { get; set; }
        public string RequestNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientMobile { get; set; } = string.Empty;
        public int? OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public DateTime RequestedDate { get; set; }
        public string RequestedTime { get; set; } = string.Empty;
        public int? AssignedAgentId { get; set; }
        public string? AssignedAgentName { get; set; }
        public HomeCollectionStatus Status { get; set; }
        public DateTime? CollectionDateTime { get; set; }
        public string? Remarks { get; set; }
    }

    public class RequestHomeCollectionRequest
    {
        public int PatientId { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public DateTime RequestedDate { get; set; }
        public string RequestedTime { get; set; } = string.Empty; // e.g. "08:00 AM - 10:00 AM"
        public string? Remarks { get; set; }
    }

    public class AssignAgentRequest
    {
        public int HomeCollectionId { get; set; }
        public int AssignedAgentId { get; set; }
    }
}
