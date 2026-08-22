using System.Collections.Generic;

namespace Patholab.Shared.DTOs
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string DepartmentCode { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class SampleTypeDto
    {
        public int Id { get; set; }
        public string SampleTypeCode { get; set; } = string.Empty;
        public string SampleTypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class TestDto
    {
        public int Id { get; set; }
        public string TestCode { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int SampleTypeId { get; set; }
        public string SampleTypeName { get; set; } = string.Empty;
        public string TestType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int TATMinutes { get; set; }
        public bool IsActive { get; set; } = true;
        public List<TestParameterDto> TestParameters { get; set; } = new List<TestParameterDto>();
    }

    public class TestParameterDto
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string ParameterCode { get; set; } = string.Empty;
        public string ParameterName { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public string ResultType { get; set; } = string.Empty;
        public string? Unit { get; set; }
        public decimal? MaleMin { get; set; }
        public decimal? MaleMax { get; set; }
        public decimal? FemaleMin { get; set; }
        public decimal? FemaleMax { get; set; }
        public decimal? ChildMin { get; set; }
        public decimal? ChildMax { get; set; }
        public string? DefaultReferenceRange { get; set; }
        public decimal? CriticalLow { get; set; }
        public decimal? CriticalHigh { get; set; }
        public bool IsRequired { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }

    public class TestPackageDto
    {
        public int Id { get; set; }
        public string PackageCode { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsActive { get; set; } = true;
        public List<int> TestIds { get; set; } = new List<int>();
        public List<TestDto> Tests { get; set; } = new List<TestDto>();
    }
}
