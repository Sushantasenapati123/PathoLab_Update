using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Patholab.Application.Interfaces;

namespace Patholab.Infrastructure.Services
{
    public class MockEmailService : IEmailService
    {
        private readonly ILogger<MockEmailService> _logger;

        public MockEmailService(ILogger<MockEmailService> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("MOCK EMAIL SENT: To={To}, Subject={Subject}, BodySummary={BodySummary}", 
                to, subject, body.Length > 60 ? body.Substring(0, 60) + "..." : body);
            return Task.CompletedTask;
        }
    }

    public class MockSmsService : ISmsService
    {
        private readonly ILogger<MockSmsService> _logger;

        public MockSmsService(ILogger<MockSmsService> logger)
        {
            _logger = logger;
        }

        public Task SendSmsAsync(string to, string message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("MOCK SMS SENT: To={To}, Message={Message}", to, message);
            return Task.CompletedTask;
        }
    }

    public class MockWhatsAppService : IWhatsAppService
    {
        private readonly ILogger<MockWhatsAppService> _logger;

        public MockWhatsAppService(ILogger<MockWhatsAppService> logger)
        {
            _logger = logger;
        }

        public Task SendWhatsAppAsync(string to, string message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("MOCK WHATSAPP SENT: To={To}, Message={Message}", to, message);
            return Task.CompletedTask;
        }
    }
}
