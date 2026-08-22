using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patholab.MVC.Services;

namespace Patholab.MVC.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly PatholabApiClient _apiClient;

        public DashboardController(PatholabApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var summary = await _apiClient.GetDashboardSummaryAsync();
                var testStats = await _apiClient.GetDashboardTestStatsAsync(6);
                var deptStats = await _apiClient.GetDashboardDepartmentStatsAsync();
                var revenue = await _apiClient.GetDashboardRevenueChartAsync(7);
                
                List<Shared.DTOs.AuditLogDto> recentAudits;
                try
                {
                    var auditsResult = await _apiClient.GetAuditLogsPagedAsync(1, 5, null);
                    recentAudits = auditsResult?.Items ?? new List<Shared.DTOs.AuditLogDto>();
                }
                catch
                {
                    recentAudits = new List<Shared.DTOs.AuditLogDto>();
                }

                ViewBag.TestStats = testStats;
                ViewBag.DeptStats = deptStats;
                ViewBag.Revenue = revenue;
                ViewBag.RecentAudits = recentAudits;

                return View(summary);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to load dashboard data: {ex.Message}";
                return View(new Shared.DTOs.DashboardSummaryDto());
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadManual()
        {
            try
            {
                var fileBytes = await _apiClient.GetManualPdfAsync();
                return File(fileBytes, "application/pdf", "Patholab_User_Manual.pdf");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to download manual: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
