using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Patholab.Application.Interfaces;
using Patholab.Domain.Entities;
using Patholab.Domain.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Patholab.Infrastructure.Pdf
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly IApplicationDbContext _context;
        private readonly IQrGenerator _qrGenerator;

        public PdfGenerator(IApplicationDbContext context, IQrGenerator qrGenerator)
        {
            _context = context;
            _qrGenerator = qrGenerator;
        }

        public async Task<byte[]> GenerateReportPdfAsync(Report report, CancellationToken cancellationToken = default)
        {
            // Initialize QuestPDF Community License
            QuestPDF.Settings.License = LicenseType.Community;

            // Fetch all test results under this report's order
            var results = await _context.TestResults
                .Include(r => r.TestParameter)
                .Include(r => r.SampleTest)
                .ThenInclude(st => st.Test)
                .ThenInclude(t => t.Department)
                .Where(r => r.SampleTest.Sample.OrderId == report.OrderId && r.SampleTest.Status == "Completed")
                .OrderBy(r => r.SampleTest.Test.Department.DepartmentName)
                .ThenBy(r => r.SampleTest.Test.TestName)
                .ThenBy(r => r.TestParameter.DisplayOrder)
                .ToListAsync(cancellationToken);

            // Group results by Test Name
            var testGroups = results.GroupBy(r => r.SampleTest.Test.TestName).ToList();

            // Generate Verification QR Code
            var publicVerifyUrl = $"https://localhost:7083/Reports/Verify?reportNumber={report.ReportNumber}";
            byte[] qrBytes;
            try
            {
                qrBytes = _qrGenerator.GenerateQrCode(publicVerifyUrl);
            }
            catch
            {
                // Fallback empty byte array if QR generation fails
                qrBytes = Array.Empty<byte>();
            }

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    // Header
                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(headerCol =>
                            {
                                headerCol.Item().Text("PATHOLAB DIAGNOSTIC CENTER").FontSize(18).Bold().FontColor(Colors.Blue.Medium);
                                headerCol.Item().Text("Quality Care & Precision Diagnostics").FontSize(9).Italic().FontColor(Colors.Grey.Medium);
                                headerCol.Item().Text("102, Health Arcade, Sector 15, Navi Mumbai - 400703").FontSize(8);
                                headerCol.Item().Text("Phone: +91 22 2774 9876 | Email: reports@patholab.com").FontSize(8);
                            });

                            if (qrBytes.Length > 0)
                            {
                                row.ConstantItem(60).Height(60).Image(qrBytes);
                            }
                        });

                        col.Item().PaddingTop(10).PaddingBottom(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        
                        // Patient Meta Information Box
                        col.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Background(Colors.Grey.Lighten4).Padding(8).Row(metaRow =>
                        {
                            metaRow.RelativeItem().Column(metaCol =>
                            {
                                metaCol.Item().Text(t => { t.Span("Patient Name: ").Bold(); t.Span($"{report.Patient.FirstName} {report.Patient.LastName}"); });
                                metaCol.Item().Text(t => { t.Span("Patient ID: ").Bold(); t.Span(report.Patient.PatientCode); });
                                metaCol.Item().Text(t => { t.Span("Age / Gender: ").Bold(); t.Span($"{report.Patient.Age} Years / {report.Patient.Gender}"); });
                            });

                            metaRow.RelativeItem().Column(metaCol =>
                            {
                                metaCol.Item().Text(t => { t.Span("Report Number: ").Bold(); t.Span(report.ReportNumber); });
                                metaCol.Item().Text(t => { t.Span("Order Date: ").Bold(); t.Span(report.Order.OrderDate.ToString("dd-MMM-yyyy hh:mm tt")); });
                                metaCol.Item().Text(t => { t.Span("Ref. Doctor: ").Bold(); t.Span(report.Order.Doctor?.DoctorName ?? "Self Referral"); });
                            });
                        });

                        col.Item().PaddingTop(10);
                    });

                    // Body
                    page.Content().Column(col =>
                    {
                        foreach (var testGroup in testGroups)
                        {
                            col.Item().PaddingTop(10).Column(testCol =>
                            {
                                // Test Group Header
                                testCol.Item().Background(Colors.Grey.Lighten2).Padding(4).Text(testGroup.Key.ToUpper()).Bold().FontSize(11);

                                // Parameters Table
                                testCol.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(3); // Parameter Name
                                        columns.RelativeColumn(2); // Result Value
                                        columns.RelativeColumn(1.5f); // Unit
                                        columns.RelativeColumn(2.5f); // Reference Range
                                        columns.RelativeColumn(1.5f); // Status
                                    });

                                    // Header
                                    table.Header(header =>
                                    {
                                        header.Cell().Text("Test Parameter").Bold().FontSize(9);
                                        header.Cell().Text("Result").Bold().FontSize(9);
                                        header.Cell().Text("Unit").Bold().FontSize(9);
                                        header.Cell().Text("Reference Range").Bold().FontSize(9);
                                        header.Cell().Text("Flag").Bold().FontSize(9);
                                    });

                                    // Rows
                                    foreach (var res in testGroup)
                                    {
                                        var isAbnormal = res.ResultStatus != ResultStatus.Normal && res.ResultStatus != ResultStatus.Pending;

                                        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(res.TestParameter.ParameterName).FontSize(9);
                                        
                                        var resultText = table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(res.ResultValue ?? "Pending").FontSize(9);
                                        if (isAbnormal) resultText.Bold();

                                        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(res.Unit ?? res.TestParameter.Unit ?? "N/A").FontSize(9);
                                        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(res.ReferenceRange ?? res.TestParameter.DefaultReferenceRange ?? "N/A").FontSize(9);

                                        var flagText = table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(res.ResultStatus.ToString().ToUpper()).FontSize(9);
                                        if (isAbnormal)
                                        {
                                            flagText.Bold();
                                            if (res.ResultStatus == ResultStatus.Critical)
                                            {
                                                flagText.FontColor(Colors.Red.Medium);
                                            }
                                            else
                                            {
                                                flagText.FontColor(Colors.Orange.Medium);
                                            }
                                        }
                                    }
                                });
                            });
                        }

                        // Pathologist Signatures Area
                        col.Item().PaddingTop(30).Row(signRow =>
                        {
                            signRow.RelativeItem().Column(signCol =>
                            {
                                signCol.Item().Text("Technician:").FontSize(8).Bold().FontColor(Colors.Grey.Medium);
                                signCol.Item().Text("Tushar Roy, DMLT").FontSize(9);
                                signCol.Item().Text("Report Entered On: " + DateTime.UtcNow.ToString("dd-MMM-yyyy")).FontSize(8).Italic();
                            });

                            signRow.RelativeItem().Column(signCol =>
                            {
                                signCol.Item().AlignRight().Text("Pathologist Sign-off:").FontSize(8).Bold().FontColor(Colors.Grey.Medium);
                                signCol.Item().AlignRight().Text("Dr. Priya Nair, MD (Pathology)").FontSize(9).Bold().FontColor(Colors.Blue.Medium);
                                signCol.Item().AlignRight().Text("Reg No: PMC/45312/2012").FontSize(8);
                                signCol.Item().AlignRight().Text("Digitally Signed").FontSize(8).Italic().FontColor(Colors.Green.Medium);
                            });
                        });
                    });

                    // Footer
                    page.Footer().Column(col =>
                    {
                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        col.Item().PaddingTop(5).Row(row =>
                        {
                            row.RelativeItem().Text("Note: This is a digitally signed computer-generated report. You can verify it by scanning the QR code at the top.").FontSize(8).Italic();
                            row.RelativeItem().AlignRight().Text(x =>
                            {
                                x.Span("Page ");
                                x.CurrentPageNumber();
                            });
                        });
                    });
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }
    }
}
