using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patholab.Domain.Entities;

namespace Patholab.Infrastructure.Persistence.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("PTH_Invoices");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("InvoiceId");
            builder.Property(x => x.InvoiceNumber).IsRequired().HasMaxLength(30);
            builder.HasIndex(x => x.InvoiceNumber).IsUnique();

            builder.Property(x => x.InvoiceStatus)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.GrossAmount).HasPrecision(18, 2);
            builder.Property(x => x.DiscountAmount).HasPrecision(18, 2);
            builder.Property(x => x.TaxAmount).HasPrecision(18, 2);
            builder.Property(x => x.NetAmount).HasPrecision(18, 2);
            builder.Property(x => x.PaidAmount).HasPrecision(18, 2);
            builder.Property(x => x.DueAmount).HasPrecision(18, 2);

            builder.HasOne(x => x.Order)
                .WithMany(o => o.Invoices)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Patient)
                .WithMany(p => p.Invoices)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.PatientId);

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("PTH_Payments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("PaymentId");
            builder.Property(x => x.PaymentNumber).IsRequired().HasMaxLength(30);
            builder.HasIndex(x => x.PaymentNumber).IsUnique();
            builder.Property(x => x.PaymentMode).IsRequired().HasMaxLength(20);
            builder.Property(x => x.TransactionReference).HasMaxLength(100);
            builder.Property(x => x.Remarks).HasMaxLength(250);
            builder.Property(x => x.ReceivedBy).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Amount).HasPrecision(18, 2);

            builder.HasOne(x => x.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }
}
