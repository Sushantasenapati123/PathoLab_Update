using System;
using BarcodeStandard;
using Patholab.Application.Interfaces;
using SkiaSharp;

namespace Patholab.Infrastructure.Services
{
    public class BarcodeGenerator : IBarcodeGenerator
    {
        public byte[] GenerateBarcode(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));

            try
            {
                var barcode = new Barcode();
                barcode.IncludeLabel = true;
                barcode.LabelFont = new SKFont(SKTypeface.FromFamilyName("Arial"), 12);
                
                // Encode into Code 128 format, 250px width, 80px height
                using var image = barcode.Encode(BarcodeStandard.Type.Code128, text, SKColors.Black, SKColors.White, 250, 80);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                
                return data.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to generate barcode: {ex.Message}", ex);
            }
        }
    }
}
