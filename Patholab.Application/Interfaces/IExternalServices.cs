using System.Threading;
using System.Threading.Tasks;
using Patholab.Domain.Entities;

namespace Patholab.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(byte[] fileBytes, string fileName, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
    }

    public interface IPdfGenerator
    {
        Task<byte[]> GenerateReportPdfAsync(Report report, CancellationToken cancellationToken = default);
    }

    public interface IBarcodeGenerator
    {
        byte[] GenerateBarcode(string text);
    }

    public interface IQrGenerator
    {
        byte[] GenerateQrCode(string text);
    }

    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    }

    public interface ISmsService
    {
        Task SendSmsAsync(string to, string message, CancellationToken cancellationToken = default);
    }

    public interface IWhatsAppService
    {
        Task SendWhatsAppAsync(string to, string message, CancellationToken cancellationToken = default);
    }
}
