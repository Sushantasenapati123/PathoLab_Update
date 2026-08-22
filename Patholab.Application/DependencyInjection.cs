using Microsoft.Extensions.DependencyInjection;
using Patholab.Application.Interfaces;
using Patholab.Application.Services;

namespace Patholab.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ISampleService, SampleService>();
            services.AddScoped<ITestResultService, TestResultService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IBillingService, BillingService>();
            services.AddScoped<IHomeCollectionService, HomeCollectionService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IAuditLogService, AuditLogService>();

            return services;
        }
    }
}
