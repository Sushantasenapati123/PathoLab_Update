using System;
using Patholab.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Patholab.Infrastructure.Pdf
{
    public class ManualPdfGenerator : IManualPdfGenerator
    {
        public byte[] GenerateManualPdf()
        {
            // Initialize QuestPDF Community License
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10.5f).LineHeight(1.4f));

                    // PAGE HEADER
                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(titleCol =>
                            {
                                titleCol.Item().Text("PATHOLAB DIAGNOSTICS")
                                    .FontFamily("Arial")
                                    .FontSize(24)
                                    .Bold()
                                    .FontColor(Colors.Grey.Darken4);

                                titleCol.Item().Text("Client Demonstration & Operating User Guide")
                                    .FontSize(11)
                                    .Italic()
                                    .FontColor(Colors.Grey.Medium);
                            });

                            row.ConstantItem(60).AlignRight().AlignMiddle().Column(logoCol =>
                            {
                                logoCol.Item().Text("LIMS")
                                    .FontSize(18)
                                    .Bold()
                                    .FontColor(Colors.Green.Medium);
                                logoCol.Item().Text("v1.0")
                                    .FontSize(8)
                                    .FontColor(Colors.Grey.Medium);
                            });
                        });

                        col.Item().PaddingTop(10).PaddingBottom(15).LineHorizontal(1.5f).LineColor(Colors.Grey.Lighten2);
                    });

                    // PAGE CONTENT
                    page.Content().Column(col =>
                    {
                        // Section 1: Introduction
                        col.Item().PaddingBottom(5).Text("1. System Overview & Introduction")
                            .FontSize(14)
                            .Bold()
                            .FontColor(Colors.Grey.Darken4);

                        col.Item().PaddingBottom(15).Text("Patholab LIMS is an enterprise-grade laboratory information management portal designed using Clean Architecture. It connects reception registration desks, sample collection points, test workstations, pathology review rooms, and client download hubs. This document provides step-by-step instructions to demonstrate the complete clinical cycle to clients.")
                            .FontColor(Colors.Grey.Darken3);

                        // Section 2: Demo Credentials
                        col.Item().PaddingBottom(5).Text("2. Seeded Account Logins")
                            .FontSize(14)
                            .Bold()
                            .FontColor(Colors.Grey.Darken4);

                        col.Item().PaddingBottom(8).Text("Log in with these credentials to simulate various roles during the live demo:")
                            .FontColor(Colors.Grey.Darken3);

                        // Table of accounts (drawn fully inline to prevent parameter type issues)
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(2.5f);
                            });

                            // Table Header
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten4).Padding(5).Text("Role Designation").Bold().FontSize(9.5f);
                                header.Cell().Background(Colors.Grey.Lighten4).Padding(5).Text("Username").Bold().FontSize(9.5f);
                                header.Cell().Background(Colors.Grey.Lighten4).Padding(5).Text("Password").Bold().FontSize(9.5f);
                                header.Cell().Background(Colors.Grey.Lighten4).Padding(5).Text("Primary Action Area").Bold().FontSize(9.5f);
                            });

                            // Row 1: Super Admin
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Super Admin").FontSize(9);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("admin").FontSize(9).FontFamily("Courier New");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Admin@123").FontSize(9).FontFamily("Courier New");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Full dashboard configuration & audit trails").FontSize(9);

                            // Row 2: Receptionist
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Receptionist").FontSize(9);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("reception").FontSize(9).FontFamily("Courier New");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Reception@123").FontSize(9).FontFamily("Courier New");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Patients registration, order booking & invoicing").FontSize(9);

                            // Row 3: Lab Technician
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Lab Technician").FontSize(9);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("technician").FontSize(9).FontFamily("Courier New");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Technician@123").FontSize(9).FontFamily("Courier New");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Vial barcode collection, receipt, & result entry").FontSize(9);

                            // Row 4: Pathologist
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Pathologist").FontSize(9);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("pathologist").FontSize(9).FontFamily("Courier New");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Pathologist@123").FontSize(9).FontFamily("Courier New");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Review flags, add remarks, sign & publish PDF").FontSize(9);

                            // Row 5: Accountant
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Accountant").FontSize(9);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("accountant").FontSize(9).FontFamily("Courier New");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Accountant@123").FontSize(9).FontFamily("Courier New");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Collect outstanding dues & financial audit").FontSize(9);

                            // Row 6: Field Agent
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Field Agent").FontSize(9);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("agent").FontSize(9).FontFamily("Courier New");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Agent@123").FontSize(9).FontFamily("Courier New");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Home sample collections schedule dispatch").FontSize(9);
                        });

                        col.Item().PaddingTop(15);

                        // Section 3: Walkthrough Flow
                        col.Item().PaddingBottom(5).Text("3. Step-by-Step Clinical Demo Cycle")
                            .FontSize(14)
                            .Bold()
                            .FontColor(Colors.Grey.Darken4);

                        // Steps list (Inlined)
                        col.Item().PaddingBottom(8).Column(stepCol =>
                        {
                            stepCol.Item().Text("Step 1: Patient Intake").Bold().FontSize(10.5f).FontColor(Colors.Grey.Darken4);
                            stepCol.Item().Text("Log in as receptionist. Navigate to Patients and click 'Register New Patient'. Enter demographic metrics (age, gender, mobile). Next, select 'Order Booking' to register a new diagnostics request. Add a referral doctor and tests (e.g. CBC, Fasting Sugar) and record the cash billing settlement.").FontSize(9.5f).FontColor(Colors.Grey.Darken2);
                        });

                        col.Item().PaddingBottom(8).Column(stepCol =>
                        {
                            stepCol.Item().Text("Step 2: Vial Draw & Lab Receipt").Bold().FontSize(10.5f).FontColor(Colors.Grey.Darken4);
                            stepCol.Item().Text("Log in as technician. Under 'Specimens Workstation', view pending collections. Click 'Collect' (assigns a unique vial barcode) and click 'Lab Receipt' to mark the vial as received in the central diagnostics area.").FontSize(9.5f).FontColor(Colors.Grey.Darken2);
                        });

                        col.Item().PaddingBottom(8).Column(stepCol =>
                        {
                            stepCol.Item().Text("Step 3: Result Entries").Bold().FontSize(10.5f).FontColor(Colors.Grey.Darken4);
                            stepCol.Item().Text("In the technician workstation, click 'Enter Results' for the received sample. Type observed numerical values (e.g., Fasting Sugar = 145 mg/dL). Click 'Lock & Request Review' to lock inputs and dispatch the case to pathology.").FontSize(9.5f).FontColor(Colors.Grey.Darken2);
                        });

                        col.Item().PaddingBottom(8).Column(stepCol =>
                        {
                            stepCol.Item().Text("Step 4: Pathologist Validation & Publish").Bold().FontSize(10.5f).FontColor(Colors.Grey.Darken4);
                            stepCol.Item().Text("Log in as pathologist. Open the pending review queue. The system automatically computes and flags out-of-range clinical indices (e.g., High Sugar). Write interpretation notes, click 'Approve & Verify', and finally click 'Sign-off & Publish' to release the PKCS#7 signed report.").FontSize(9.5f).FontColor(Colors.Grey.Darken2);
                        });

                        col.Item().PaddingBottom(8).Column(stepCol =>
                        {
                            stepCol.Item().Text("Step 5: Patient Receipt & QR Scan").Bold().FontSize(10.5f).FontColor(Colors.Grey.Darken4);
                            stepCol.Item().Text("Go to 'Reports Directory' to download the compiled PDF report. Scan the printed QR code using any mobile device; it routes directly to the public secure Patholab validation card, verifying report authenticity.").FontSize(9.5f).FontColor(Colors.Grey.Darken2);
                        });
                    });

                    // PAGE FOOTER
                    page.Footer().Column(col =>
                    {
                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        col.Item().PaddingTop(5).Row(row =>
                        {
                            row.RelativeItem().Text("Confidential - For Demonstration Purposes Only")
                                .FontSize(8.5f)
                                .FontColor(Colors.Grey.Medium);

                            row.RelativeItem().AlignRight().Text(x =>
                            {
                                x.Span("Page ").FontSize(8.5f).FontColor(Colors.Grey.Medium);
                                x.CurrentPageNumber().FontSize(8.5f).FontColor(Colors.Grey.Medium).Bold();
                            });
                        });
                    });
                });
            });

            using var memoryStream = new System.IO.MemoryStream();
            document.GeneratePdf(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
