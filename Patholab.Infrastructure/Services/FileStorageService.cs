using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Patholab.Application.Interfaces;

namespace Patholab.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _uploadFolder;

        public FileStorageService()
        {
            // Resolve relative path to Patholab.API wwwroot/uploads directory
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // Go up to the solution directory and point to Patholab.API/wwwroot/uploads
            // Or fallback to current directory uploads if API project is not running
            var solutionDir = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName;
            if (solutionDir != null && Directory.Exists(Path.Combine(solutionDir, "Patholab.API")))
            {
                _uploadFolder = Path.Combine(solutionDir, "Patholab.API", "wwwroot", "uploads");
            }
            else
            {
                _uploadFolder = Path.Combine(baseDirectory, "wwwroot", "uploads");
            }

            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        public async Task<string> SaveFileAsync(byte[] fileBytes, string fileName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("Filename cannot be null or empty.");

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(_uploadFolder, uniqueFileName);

            await File.WriteAllBytesAsync(filePath, fileBytes, cancellationToken);

            // Return relative path for web consumption (e.g. /uploads/filename.pdf)
            return $"/uploads/{uniqueFileName}";
        }

        public Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(filePath)) return Task.CompletedTask;

            var fileName = Path.GetFileName(filePath);
            var fullPath = Path.Combine(_uploadFolder, fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }
    }
}
