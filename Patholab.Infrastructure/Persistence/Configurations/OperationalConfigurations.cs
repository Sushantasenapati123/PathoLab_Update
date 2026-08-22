using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patholab.Domain.Entities;

namespace Patholab.Infrastructure.Persistence.Configurations
{
    public class HomeCollectionConfiguration : IEntityTypeConfiguration<HomeCollection>
    {
        public void Configure(EntityTypeBuilder<HomeCollection> builder)
        {
            builder.ToTable("PTH_HomeCollections");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("HomeCollectionId");
            builder.Property(x => x.RequestNumber).IsRequired().HasMaxLength(30);
            builder.HasIndex(x => x.RequestNumber).IsUnique();
            builder.Property(x => x.Address).IsRequired().HasMaxLength(250);
            builder.Property(x => x.City).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Pincode).IsRequired().HasMaxLength(10);
            builder.Property(x => x.RequestedTime).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Remarks).HasMaxLength(250);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(x => x.Patient)
                .WithMany(p => p.HomeCollections)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Order)
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AssignedAgent)
                .WithMany()
                .HasForeignKey(x => x.AssignedAgentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("PTH_Notifications");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("NotificationId");
            builder.Property(x => x.Recipient).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Subject).HasMaxLength(150);
            builder.Property(x => x.Message).IsRequired();
            builder.Property(x => x.ReferenceType).HasMaxLength(50);
            builder.Property(x => x.FailureReason).HasMaxLength(500);

            builder.Property(x => x.NotificationType)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(x => x.Patient)
                .WithMany()
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("PTH_AuditLogs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("AuditLogId");
            builder.Property(x => x.ModuleName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Action).IsRequired().HasMaxLength(50);
            builder.Property(x => x.TableName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.IPAddress).HasMaxLength(50);
            builder.Property(x => x.UserAgent).HasMaxLength(250);

            builder.HasOne(x => x.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
