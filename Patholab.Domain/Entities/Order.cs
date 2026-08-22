using System;
using System.Collections.Generic;
using Patholab.Domain.Enums;

namespace Patholab.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; } = null!;
        public int? DoctorId { get; set; }
        public virtual Doctor? Doctor { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Registered;
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
        public string? Remarks { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
