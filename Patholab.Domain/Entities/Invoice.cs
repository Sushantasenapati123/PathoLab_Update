using System;
using System.Collections.Generic;
using Patholab.Domain.Enums;

namespace Patholab.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; } = null!;
        public DateTime InvoiceDate { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; } = InvoiceStatus.Unpaid;

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
