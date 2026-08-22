using System;
using Patholab.Application.Interfaces;
using QRCoder;

namespace Patholab.Infrastructure.Services
{
    public class QrGenerator : IQrGenerator
    {
        public byte[] GenerateQrCode(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));

            try
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new PngByteQRCode(qrCodeData);
                
                // Get graphic with scale 10
                return qrCode.GetGraphic(10);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to generate QR code: {ex.Message}", ex);
            }
        }
    }
}
