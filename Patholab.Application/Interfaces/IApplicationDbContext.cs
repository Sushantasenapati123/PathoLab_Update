using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Patholab.Domain.Entities;

namespace Patholab.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<Permission> Permissions { get; }
        DbSet<RolePermission> RolePermissions { get; }
        DbSet<Patient> Patients { get; }
        DbSet<Doctor> Doctors { get; }
        DbSet<Department> Departments { get; }
        DbSet<SampleType> SampleTypes { get; }
        DbSet<Test> Tests { get; }
        DbSet<TestParameter> TestParameters { get; }
        DbSet<TestPackage> TestPackages { get; }
        DbSet<PackageTest> PackageTests { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderDetail> OrderDetails { get; }
        DbSet<Sample> Samples { get; }
        DbSet<SampleTest> SampleTests { get; }
        DbSet<TestResult> TestResults { get; }
        DbSet<TestResultHistory> TestResultHistories { get; }
        DbSet<Report> Reports { get; }
        DbSet<ReportDetail> ReportDetails { get; }
        DbSet<Invoice> Invoices { get; }
        DbSet<Payment> Payments { get; }
        DbSet<HomeCollection> HomeCollections { get; }
        DbSet<Notification> Notifications { get; }
        DbSet<AuditLog> AuditLogs { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
