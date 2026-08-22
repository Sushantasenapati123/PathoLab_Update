using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patholab.MVC.Services;

namespace Patholab.MVC.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly PatholabApiClient _apiClient;

        public ReportsController(PatholabApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index(string? search = null, int pageNumber = 1, int pageSize = 15)
        {
            try
            {
                var result = await _apiClient.GetReportsPagedAsync(pageNumber, pageSize, search);
                ViewBag.Search = search;
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new Shared.Models.PagedResult<Shared.DTOs.ReportDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Download(int id)
        {
            try
            {
                var pdfBytes = await _apiClient.DownloadReportPdfAsync(id);
                var report = await _apiClient.GetReportByIdAsync(id);
                return File(pdfBytes, "application/pdf", $"Report_{report.ReportNumber}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to download PDF: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [AllowAnonymous] // Open to everyone who scans the printed QR Code!
        public async Task<IActionResult> Verify(string reportNumber)
        {
            if (string.IsNullOrEmpty(reportNumber))
            {
                ViewBag.Error = "Invalid report verification scan. Report number missing.";
                return View(new Shared.DTOs.ReportVerificationResponse());
            }

            try
            {
                var response = await _apiClient.VerifyPublicReportAsync(reportNumber);
                return View(response);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new Shared.DTOs.ReportVerificationResponse());
            }
        }
    }
}
