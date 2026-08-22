using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patholab.MVC.Services;
using Patholab.Shared.DTOs;

namespace Patholab.MVC.Controllers
{
    [Authorize]
    public class LaboratoryController : Controller
    {
        private readonly PatholabApiClient _apiClient;

        public LaboratoryController(PatholabApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // ==========================================
        // SAMPLE COLLECTIONS & RECEPTIONS
        // ==========================================

        public async Task<IActionResult> Samples(string? search = null, int pageNumber = 1, int pageSize = 15)
        {
            try
            {
                var result = await _apiClient.GetSamplesPagedAsync(pageNumber, pageSize, search);
                ViewBag.Search = search;
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new Shared.Models.PagedResult<SampleDto>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Collect(int sampleId, string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
            {
                TempData["Error"] = "Barcode value is required.";
                return RedirectToAction(nameof(Samples));
            }

            try
            {
                var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int.TryParse(userIdStr, out int userId);

                await _apiClient.CollectSampleAsync(sampleId, barcode, userId);
                TempData["Success"] = "Specimen collected successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Samples));
        }

        [HttpPost]
        public async Task<IActionResult> Receive(int sampleId)
        {
            try
            {
                var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int.TryParse(userIdStr, out int userId);

                await _apiClient.ReceiveSampleAsync(sampleId, userId);
                TempData["Success"] = "Specimen received in laboratory. Processing started.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Samples));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int sampleId, string reason, string? remarks)
        {
            if (string.IsNullOrEmpty(reason))
            {
                TempData["Error"] = "Rejection reason is required.";
                return RedirectToAction(nameof(Samples));
            }

            try
            {
                var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int.TryParse(userIdStr, out int userId);

                await _apiClient.RejectSampleAsync(sampleId, reason, remarks, userId);
                TempData["Success"] = "Sample rejected successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Samples));
        }

        // ==========================================
        // CLINICAL RESULT ENTRY
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> ResultEntry(int sampleId)
        {
            try
            {
                var sample = await _apiClient.GetSampleByIdAsync(sampleId);
                var results = await _apiClient.GetResultsBySampleIdAsync(sampleId);
                
                ViewBag.Sample = sample;
                ViewBag.Results = results;
                
                var model = new EnterResultsRequest { SampleId = sampleId };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Samples));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnterResults(EnterResultsRequest model, List<int> paramIds, List<string> paramValues, List<string> paramRemarks)
        {
            // Reconstruct the nested list from form data collections
            model.Results = new List<EnterResultItem>();
            for (int i = 0; i < paramIds.Count; i++)
            {
                model.Results.Add(new EnterResultItem
                {
                    TestParameterId = paramIds[i],
                    ResultValue = paramValues[i],
                    Remarks = paramRemarks.Count > i ? paramRemarks[i] : null
                });
            }

            try
            {
                await _apiClient.EnterResultsAsync(model);
                TempData["Success"] = "Test results saved successfully.";
                return RedirectToAction(nameof(ResultEntry), new { sampleId = model.SampleId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(ResultEntry), new { sampleId = model.SampleId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitResults(int sampleId)
        {
            try
            {
                await _apiClient.SubmitResultsForVerificationAsync(sampleId);
                TempData["Success"] = "Results locked and submitted for pathology verification.";
                return RedirectToAction(nameof(Samples));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(ResultEntry), new { sampleId });
            }
        }

        // ==========================================
        // PATHOLOGIST VERIFICATION & SIGN-OFF
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Verification(string? search = null, int pageNumber = 1)
        {
            try
            {
                // Renders the list of reports under pathologist review
                var result = await _apiClient.GetReportsPagedAsync(pageNumber, 15, search);
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new Shared.Models.PagedResult<ReportDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> VerifyDetails(int id)
        {
            try
            {
                var report = await _apiClient.GetReportByIdAsync(id);
                // Load results of the order
                // Find matching blood/serum sample ID for the order
                var samplesResult = await _apiClient.GetSamplesPagedAsync(1, 50, report.OrderNumber);
                var sampleResultsMap = new Dictionary<string, List<TestResultDto>>();
                
                foreach (var sample in samplesResult.Items)
                {
                    var resList = await _apiClient.GetResultsBySampleIdAsync(sample.Id);
                    sampleResultsMap[sample.SampleTypeName] = resList;
                }

                ViewBag.Samples = samplesResult.Items;
                ViewBag.SampleResultsMap = sampleResultsMap;
                return View(report);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Verification));
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyReport(int reportId, string? remarks)
        {
            try
            {
                var report = await _apiClient.VerifyReportAsync(reportId, remarks);
                TempData["Success"] = $"Report {report.ReportNumber} verified by pathologist successfully.";
                return RedirectToAction(nameof(VerifyDetails), new { id = reportId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(VerifyDetails), new { id = reportId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PublishReport(int reportId)
        {
            try
            {
                var report = await _apiClient.PublishReportAsync(reportId);
                TempData["Success"] = $"Report {report.ReportNumber} published and PDF generated successfully.";
                return RedirectToAction(nameof(VerifyDetails), new { id = reportId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(VerifyDetails), new { id = reportId });
            }
        }
    }
}
