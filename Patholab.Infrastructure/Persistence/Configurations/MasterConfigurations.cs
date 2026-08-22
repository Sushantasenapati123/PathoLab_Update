using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patholab.Domain.Entities;

namespace Patholab.Infrastructure.Persistence.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("PTH_Patients");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("PatientId");
            builder.Property(x => x.PatientCode).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.PatientCode).IsUnique();
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.MiddleName).HasMaxLength(50);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Gender).IsRequired().HasMaxLength(10);
            builder.Property(x => x.BloodGroup).HasMaxLength(5);
            builder.Property(x => x.Mobile).IsRequired().HasMaxLength(15);
            builder.HasIndex(x => x.Mobile);
            builder.Property(x => x.AlternateMobile).HasMaxLength(15);
            builder.Property(x => x.Email).HasMaxLength(100);
            builder.Property(x => x.Address).HasMaxLength(250);
            builder.Property(x => x.City).HasMaxLength(100);
            builder.Property(x => x.State).HasMaxLength(100);
            builder.Property(x => x.Pincode).HasMaxLength(10);
            builder.Property(x => x.EmergencyContactName).HasMaxLength(100);
            builder.Property(x => x.EmergencyContactMobile).HasMaxLength(15);
            builder.Property(x => x.IdentityType).HasMaxLength(50);
            builder.Property(x => x.IdentityNumber).HasMaxLength(50);
            builder.Property(x => x.Remarks).HasMaxLength(500);

            // Composite index for searching name
            builder.HasIndex(x => new { x.FirstName, x.LastName });

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("PTH_Doctors");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("DoctorId");
            builder.Property(x => x.DoctorCode).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.DoctorCode).IsUnique();
            builder.Property(x => x.DoctorName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Qualification).HasMaxLength(100);
            builder.Property(x => x.Specialization).HasMaxLength(100);
            builder.Property(x => x.HospitalClinicName).HasMaxLength(200);
            builder.Property(x => x.Mobile).IsRequired().HasMaxLength(15);
            builder.Property(x => x.Email).HasMaxLength(100);
            builder.Property(x => x.Address).HasMaxLength(250);
            
            builder.Property(x => x.CommissionType)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.CommissionValue).HasPrecision(18, 2);

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("PTH_Departments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("DepartmentId");
            builder.Property(x => x.DepartmentCode).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.DepartmentCode).IsUnique();
            builder.Property(x => x.DepartmentName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(250);
            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class SampleTypeConfiguration : IEntityTypeConfiguration<SampleType>
    {
        public void Configure(EntityTypeBuilder<SampleType> builder)
        {
            builder.ToTable("PTH_SampleTypes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("SampleTypeId");
            builder.Property(x => x.SampleTypeCode).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.SampleTypeCode).IsUnique();
            builder.Property(x => x.SampleTypeName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(250);
            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }
}
