using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patholab.Application.Interfaces;
using Patholab.Infrastructure.Persistence;
using Patholab.Infrastructure.Services;
using Patholab.Infrastructure.Pdf;
using Biwen.EFCore.UseRowNumberForPaging;

namespace Patholab.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Register DbContext
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? "Server=(localdb)\\mssqllocaldb;Database=PatholabDB;Trusted_Connection=True;MultipleActiveResultSets=true";

            services.AddDbContext<PatholabDbContext>(options =>
                options.UseSqlServer(connectionString, b => b
                    .MigrationsAssembly("Patholab.Infrastructure")
                    .UseRowNumberForPaging()));

            // Map IApplicationDbContext to PatholabDbContext
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<PatholabDbContext>());

            // 2. Register Infrastructure Services
            services.AddSingleton<IFileStorageService, FileStorageService>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient<IBarcodeGenerator, BarcodeGenerator>();
            services.AddTransient<IQrGenerator, QrGenerator>();
            services.AddTransient<IPdfGenerator, PdfGenerator>();
            services.AddTransient<IManualPdfGenerator, ManualPdfGenerator>();

            // 3. Register Mock Notification Services
            services.AddTransient<IEmailService, MockEmailService>();
            services.AddTransient<ISmsService, MockSmsService>();
            services.AddTransient<IWhatsAppService, MockWhatsAppService>();

            return services;
        }
    }
}
