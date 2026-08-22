using System;
using System.Collections.Generic;
using Patholab.Domain.Enums;

namespace Patholab.Shared.DTOs
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientCode { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
        public List<PaymentDto> Payments { get; set; } = new List<PaymentDto>();
    }

    public class PaymentDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string PaymentNumber { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string? TransactionReference { get; set; }
        public string? Remarks { get; set; }
        public string ReceivedBy { get; set; } = string.Empty;
    }

    public class CreatePaymentRequest
    {
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; } = string.Empty; // Cash, UPI, Card, etc.
        public string? TransactionReference { get; set; }
        public string? Remarks { get; set; }
    }
}
