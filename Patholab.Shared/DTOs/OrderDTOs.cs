using System;
using System.Collections.Generic;
using Patholab.Domain.Enums;

namespace Patholab.Shared.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientCode { get; set; } = string.Empty;
        public string PatientMobile { get; set; } = string.Empty;
        public int? DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? Remarks { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
        public List<SampleDto> Samples { get; set; } = new List<SampleDto>();
    }

    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? TestId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public string TestCode { get; set; } = string.Empty;
        public int? PackageId { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public string PackageCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public bool SampleRequired { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateOrderRequest
    {
        public int PatientId { get; set; }
        public int? DoctorId { get; set; }
        public List<int> TestIds { get; set; } = new List<int>();
        public List<int> PackageIds { get; set; } = new List<int>();
        public decimal DiscountAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentMode { get; set; } = string.Empty; // Cash, UPI, Card, etc.
        public string? TransactionReference { get; set; }
        public string? Remarks { get; set; }
        public List<CustomRateItem>? CustomRates { get; set; }
    }

    public class CustomRateItem
    {
        public int? TestId { get; set; }
        public int? PackageId { get; set; }
        public decimal Rate { get; set; }
    }
}
