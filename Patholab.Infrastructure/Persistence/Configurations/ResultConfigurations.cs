using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patholab.Domain.Entities;

namespace Patholab.Infrastructure.Persistence.Configurations
{
    public class TestResultConfiguration : IEntityTypeConfiguration<TestResult>
    {
        public void Configure(EntityTypeBuilder<TestResult> builder)
        {
            builder.ToTable("PTH_TestResults");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("TestResultId");
            builder.Property(x => x.ResultValue).HasMaxLength(200);
            builder.Property(x => x.ResultNumericValue).HasPrecision(18, 4);
            
            builder.Property(x => x.ResultStatus)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.ReferenceRange).HasMaxLength(200);
            builder.Property(x => x.Unit).HasMaxLength(50);
            builder.Property(x => x.Remarks).HasMaxLength(500);

            builder.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            builder.HasOne(x => x.SampleTest)
                .WithMany(s => s.TestResults)
                .HasForeignKey(x => x.SampleTestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.TestParameter)
                .WithMany(tp => tp.TestResults)
                .HasForeignKey(x => x.TestParameterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.EnteredBy)
                .WithMany()
                .HasForeignKey(x => x.EnteredById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.VerifiedBy)
                .WithMany()
                .HasForeignKey(x => x.VerifiedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class TestResultHistoryConfiguration : IEntityTypeConfiguration<TestResultHistory>
    {
        public void Configure(EntityTypeBuilder<TestResultHistory> builder)
        {
            builder.ToTable("PTH_TestResultHistory");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("HistoryId");
            builder.Property(x => x.OldValue).HasMaxLength(200);
            builder.Property(x => x.NewValue).HasMaxLength(200);
            builder.Property(x => x.OldStatus).HasMaxLength(20);
            builder.Property(x => x.NewStatus).HasMaxLength(20);
            builder.Property(x => x.Reason).IsRequired().HasMaxLength(250);

            builder.HasOne(x => x.TestResult)
                .WithMany(r => r.TestResultHistories)
                .HasForeignKey(x => x.TestResultId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ChangedBy)
                .WithMany()
                .HasForeignKey(x => x.ChangedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("PTH_Reports");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ReportId");
            builder.Property(x => x.ReportNumber).IsRequired().HasMaxLength(30);
            builder.HasIndex(x => x.ReportNumber).IsUnique();
            builder.Property(x => x.PdfFilePath).HasMaxLength(500);
            builder.Property(x => x.Remarks).HasMaxLength(500);

            builder.Property(x => x.ReportStatus)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(x => x.Order)
                .WithMany(o => o.Reports)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Patient)
                .WithMany(p => p.Reports)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.VerifiedBy)
                .WithMany()
                .HasForeignKey(x => x.VerifiedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.PatientId);

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class ReportDetailConfiguration : IEntityTypeConfiguration<ReportDetail>
    {
        public void Configure(EntityTypeBuilder<ReportDetail> builder)
        {
            builder.ToTable("PTH_ReportDetails");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ReportDetailId");
            builder.Property(x => x.TestName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Remarks).HasMaxLength(500);

            builder.HasOne(x => x.Report)
                .WithMany(r => r.ReportDetails)
                .HasForeignKey(x => x.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Test)
                .WithMany(t => t.ReportDetails)
                .HasForeignKey(x => x.TestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
