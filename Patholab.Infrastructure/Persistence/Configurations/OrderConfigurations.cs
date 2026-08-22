using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patholab.Domain.Entities;

namespace Patholab.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("PTH_Orders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("OrderId");
            builder.Property(x => x.OrderNumber).IsRequired().HasMaxLength(30);
            builder.HasIndex(x => x.OrderNumber).IsUnique();
            builder.Property(x => x.Remarks).HasMaxLength(500);

            builder.Property(x => x.OrderStatus)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(x => x.PaymentStatus)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
            builder.Property(x => x.DiscountAmount).HasPrecision(18, 2);
            builder.Property(x => x.NetAmount).HasPrecision(18, 2);
            builder.Property(x => x.PaidAmount).HasPrecision(18, 2);
            builder.Property(x => x.DueAmount).HasPrecision(18, 2);

            builder.HasOne(x => x.Patient)
                .WithMany(p => p.Orders)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Doctor)
                .WithMany(d => d.Orders)
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.PatientId);
            builder.HasIndex(x => x.OrderDate);

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("PTH_OrderDetails");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("OrderDetailId");
            builder.Property(x => x.Status).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Rate).HasPrecision(18, 2);
            builder.Property(x => x.Discount).HasPrecision(18, 2);
            builder.Property(x => x.Amount).HasPrecision(18, 2);

            builder.HasOne(x => x.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Test)
                .WithMany(t => t.OrderDetails)
                .HasForeignKey(x => x.TestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TestPackage)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(x => x.PackageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class SampleConfiguration : IEntityTypeConfiguration<Sample>
    {
        public void Configure(EntityTypeBuilder<Sample> builder)
        {
            builder.ToTable("PTH_Samples");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("SampleId");
            builder.Property(x => x.SampleNumber).IsRequired().HasMaxLength(30);
            builder.HasIndex(x => x.SampleNumber).IsUnique();
            builder.Property(x => x.Barcode).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Barcode).IsUnique();
            builder.Property(x => x.RejectionReason).HasMaxLength(250);
            builder.Property(x => x.Remarks).HasMaxLength(250);

            builder.Property(x => x.SampleStatus)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(x => x.Order)
                .WithMany(o => o.Samples)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Patient)
                .WithMany(p => p.Samples)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SampleType)
                .WithMany(s => s.Samples)
                .HasForeignKey(x => x.SampleTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CollectedBy)
                .WithMany()
                .HasForeignKey(x => x.CollectedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ReceivedBy)
                .WithMany()
                .HasForeignKey(x => x.ReceivedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.PatientId);

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class SampleTestConfiguration : IEntityTypeConfiguration<SampleTest>
    {
        public void Configure(EntityTypeBuilder<SampleTest> builder)
        {
            builder.ToTable("PTH_SampleTests");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("SampleTestId");
            builder.Property(x => x.Status).IsRequired().HasMaxLength(30);

            builder.HasOne(x => x.Sample)
                .WithMany(s => s.SampleTests)
                .HasForeignKey(x => x.SampleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.OrderDetail)
                .WithMany(o => o.SampleTests)
                .HasForeignKey(x => x.OrderDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Test)
                .WithMany(t => t.SampleTests)
                .HasForeignKey(x => x.TestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AssignedTo)
                .WithMany()
                .HasForeignKey(x => x.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
