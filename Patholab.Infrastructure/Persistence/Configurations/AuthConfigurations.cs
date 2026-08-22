using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patholab.Domain.Entities;

namespace Patholab.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("PTH_Roles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("RoleId");
            builder.Property(x => x.RoleName).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.RoleName).IsUnique();
            builder.Property(x => x.Description).HasMaxLength(250);
            builder.Property(x => x.CreatedBy).HasMaxLength(100);
            builder.Property(x => x.UpdatedBy).HasMaxLength(100);
            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }

    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("PTH_Permissions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("PermissionId");
            builder.Property(x => x.PermissionCode).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.PermissionCode).IsUnique();
            builder.Property(x => x.PermissionName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ModuleName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(250);
        }
    }

    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("PTH_RolePermissions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("RolePermissionId");
            builder.HasOne(x => x.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("PTH_Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("UserId");
            builder.Property(x => x.Username).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Username).IsUnique();
            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(500);
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Mobile).IsRequired().HasMaxLength(15);
            builder.Property(x => x.CreatedBy).HasMaxLength(100);
            builder.Property(x => x.UpdatedBy).HasMaxLength(100);

            builder.HasOne(x => x.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.DeletedFlag);
        }
    }
}
