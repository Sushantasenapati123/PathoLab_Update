using System;

namespace Patholab.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; } = null!;
        public string PaymentNumber { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; } = string.Empty; // Cash, UPI, Card, BankTransfer, Online
        public string? TransactionReference { get; set; }
        public string? Remarks { get; set; }
        public string ReceivedBy { get; set; } = string.Empty;
    }
}
