using System;
using System.Collections.Generic;
using Patholab.Domain.Enums;

namespace Patholab.Shared.DTOs
{
    public class SampleDto
    {
        public int Id { get; set; }
        public string SampleNumber { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientCode { get; set; } = string.Empty;
        public int SampleTypeId { get; set; }
        public string SampleTypeName { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public DateTime? CollectionDateTime { get; set; }
        public string? CollectedByName { get; set; }
        public DateTime? ReceivedDateTime { get; set; }
        public string? ReceivedByName { get; set; }
        public SampleStatus SampleStatus { get; set; }
        public string? RejectionReason { get; set; }
        public string? Remarks { get; set; }
        public List<SampleTestDto> SampleTests { get; set; } = new List<SampleTestDto>();
    }

    public class SampleTestDto
    {
        public int Id { get; set; }
        public int SampleId { get; set; }
        public string SampleNumber { get; set; } = string.Empty;
        public int OrderDetailId { get; set; }
        public int TestId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? AssignedToName { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
    }

    public class CollectSampleRequest
    {
        public int OrderId { get; set; }
        public int PatientId { get; set; }
        public int SampleTypeId { get; set; }
        public string? Remarks { get; set; }
    }

    public class ReceiveSampleRequest
    {
        public int SampleId { get; set; }
        public string? Remarks { get; set; }
    }

    public class RejectSampleRequest
    {
        public int SampleId { get; set; }
        public string RejectionReason { get; set; } = string.Empty;
        public string? Remarks { get; set; }
    }
}
