using System;
using System.Collections.Generic;
using Patholab.Domain.Enums;

namespace Patholab.Shared.DTOs
{
    public class TestResultDto
    {
        public int Id { get; set; }
        public int SampleTestId { get; set; }
        public int TestParameterId { get; set; }
        public string ParameterName { get; set; } = string.Empty;
        public string ParameterCode { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public string? ResultValue { get; set; }
        public decimal? ResultNumericValue { get; set; }
        public string? ResultText { get; set; }
        public ResultStatus ResultStatus { get; set; }
        public string? ReferenceRange { get; set; }
        public string? Unit { get; set; }
        public string? Remarks { get; set; }
        public bool IsCritical { get; set; }
        public string? EnteredByName { get; set; }
        public DateTime? EnteredOn { get; set; }
        public string? Specimen { get; set; }
        public string? VerifiedByName { get; set; }
        public DateTime? VerifiedOn { get; set; }
    }

    public class EnterResultsRequest
    {
        public int SampleId { get; set; }
        public List<EnterResultItem> Results { get; set; } = new List<EnterResultItem>();
    }

    public class EnterResultItem
    {
        public int TestParameterId { get; set; }
        public string? ResultValue { get; set; }
        public string? Remarks { get; set; }
    }

    public class TestResultHistoryDto
    {
        public int Id { get; set; }
        public int TestResultId { get; set; }
        public string ParameterName { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? OldStatus { get; set; }
        public string? NewStatus { get; set; }
        public string ChangedByName { get; set; } = string.Empty;
        public DateTime ChangedOn { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
