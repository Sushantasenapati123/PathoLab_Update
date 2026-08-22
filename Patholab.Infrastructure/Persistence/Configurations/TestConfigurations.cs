using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patholab.Domain.Entities;

namespace Patholab.Infrastructure.Persistence.Configurations
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable("PTH_Tests");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("TestId");
            builder.Property(x => x.TestCode).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.TestCode).IsUnique();
            builder.Property(x => x.TestName).IsRequired().HasMaxLength(150);
            builder.HasIndex(x => x.TestName);
            builder.Property(x => x.TestType).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Price).HasPrecision(18, 2);

            builder.HasOne(x => x.Department)
                .WithMany(d => d.Tests)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SampleType)
                .WithMany()
                .HasForeignKey(x => x.SampleTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class TestParameterConfiguration : IEntityTypeConfiguration<TestParameter>
    {
        public void Configure(EntityTypeBuilder<TestParameter> builder)
        {
            builder.ToTable("PTH_TestParameters");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("TestParameterId");
            builder.Property(x => x.ParameterCode).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ParameterName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.ResultType).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Unit).HasMaxLength(50);
            builder.Property(x => x.DefaultReferenceRange).HasMaxLength(200);

            // Precision for ranges (18, 4)
            builder.Property(x => x.MaleMin).HasPrecision(18, 4);
            builder.Property(x => x.MaleMax).HasPrecision(18, 4);
            builder.Property(x => x.FemaleMin).HasPrecision(18, 4);
            builder.Property(x => x.FemaleMax).HasPrecision(18, 4);
            builder.Property(x => x.ChildMin).HasPrecision(18, 4);
            builder.Property(x => x.ChildMax).HasPrecision(18, 4);
            builder.Property(x => x.CriticalLow).HasPrecision(18, 4);
            builder.Property(x => x.CriticalHigh).HasPrecision(18, 4);

            builder.HasOne(x => x.Test)
                .WithMany(t => t.TestParameters)
                .HasForeignKey(x => x.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class TestPackageConfiguration : IEntityTypeConfiguration<TestPackage>
    {
        public void Configure(EntityTypeBuilder<TestPackage> builder)
        {
            builder.ToTable("PTH_TestPackages");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("PackageId");
            builder.Property(x => x.PackageCode).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.PackageCode).IsUnique();
            builder.Property(x => x.PackageName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.OriginalPrice).HasPrecision(18, 2);
            builder.Property(x => x.PackagePrice).HasPrecision(18, 2);
            builder.Property(x => x.DiscountAmount).HasPrecision(18, 2);

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class PackageTestConfiguration : IEntityTypeConfiguration<PackageTest>
    {
        public void Configure(EntityTypeBuilder<PackageTest> builder)
        {
            builder.ToTable("PTH_PackageTests");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("PackageTestId");

            builder.HasOne(x => x.TestPackage)
                .WithMany(p => p.PackageTests)
                .HasForeignKey(x => x.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Test)
                .WithMany(t => t.PackageTests)
                .HasForeignKey(x => x.TestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
