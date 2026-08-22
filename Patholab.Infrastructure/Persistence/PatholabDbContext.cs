using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Patholab.Application.Interfaces;
using Patholab.Domain.Entities;
using Patholab.Infrastructure.Persistence.Configurations;

namespace Patholab.Infrastructure.Persistence
{
    public class PatholabDbContext : DbContext, IApplicationDbContext
    {
        private IDbContextTransaction? _currentTransaction;

        public PatholabDbContext(DbContextOptions<PatholabDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<SampleType> SampleTypes => Set<SampleType>();
        public DbSet<Test> Tests => Set<Test>();
        public DbSet<TestParameter> TestParameters => Set<TestParameter>();
        public DbSet<TestPackage> TestPackages => Set<TestPackage>();
        public DbSet<PackageTest> PackageTests => Set<PackageTest>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
        public DbSet<Sample> Samples => Set<Sample>();
        public DbSet<SampleTest> SampleTests => Set<SampleTest>();
        public DbSet<TestResult> TestResults => Set<TestResult>();
        public DbSet<TestResultHistory> TestResultHistories => Set<TestResultHistory>();
        public DbSet<Report> Reports => Set<Report>();
        public DbSet<ReportDetail> ReportDetails => Set<ReportDetail>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<HomeCollection> HomeCollections => Set<HomeCollection>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations from assemblies
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PatholabDbContext).Assembly);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null) return;
            _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync(cancellationToken);
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(cancellationToken);
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
