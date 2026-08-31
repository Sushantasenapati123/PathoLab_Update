using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Patholab.MVC.Services;
using Patholab.Shared.DTOs;
using Patholab.Shared.Models;

namespace Patholab.MVC.Controllers
{
    [Authorize]
    public class NewEntryController : Controller
    {
        private readonly PatholabApiClient _apiClient;

        public NewEntryController(PatholabApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id = null)
        {
            try
            {
                // Fetch basic setup lists
                var doctorsResult = await _apiClient.GetDoctorsPagedAsync(1, 1000, null);
                var testsResult = await _apiClient.GetTestsPagedAsync(1, 1000, null);
                var packages = await _apiClient.GetPackagesAsync();

                ViewBag.Doctors = doctorsResult.Items.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.DoctorName} ({d.Specialization})"
                }).ToList();

                ViewBag.Tests = testsResult.Items;
                ViewBag.Packages = packages;

                if (id.HasValue)
                {
                    // EDIT/UPDATE MODE
                    var order = await _apiClient.GetOrderByIdAsync(id.Value);
                    ViewBag.Order = order;

                    // Fetch invoice details
                    try
                    {
                        var invoice = await _apiClient.GetInvoiceByIdAsync(id.Value);
                        ViewBag.Invoice = invoice;
                    }
                    catch
                    {
                        ViewBag.Invoice = null;
                    }

                    // Try fetching report if results have been entered/submitted
                    try
                    {
                        var report = await _apiClient.GetReportByOrderIdAsync(id.Value);
                        ViewBag.Report = report;
                    }
                    catch
                    {
                        ViewBag.Report = null;
                    }

                    // Fetch patient details to populate inputs in Edit mode
                    try
                    {
                        var patient = await _apiClient.GetPatientByIdAsync(order.PatientId);
                        ViewBag.Patient = patient;
                    }
                    catch
                    {
                        ViewBag.Patient = null;
                    }

                    return View(order);
                }

                // CREATE MODE
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to load New Entry page: " + ex.Message;
                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPatient(int id)
        {
            try
            {
                var patient = await _apiClient.GetPatientByIdAsync(id);
                return Json(new { success = true, data = patient });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveDoctor([FromBody] DoctorDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.DoctorName))
                {
                    return Json(new { success = false, message = "Doctor name is required." });
                }
                if (string.IsNullOrEmpty(dto.Mobile))
                {
                    dto.Mobile = "9999999999"; // default placeholder
                }
                var newDoc = await _apiClient.CreateDoctorAsync(dto);
                return Json(new { success = true, data = newDoc });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder([FromBody] NewEntrySaveRequest request)
        {
            try
            {
                var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int.TryParse(userIdStr, out int userId);

                // 1. Resolve Patient
                int patientId = request.PatientId;
                if (patientId == 0 && request.PatientDetails != null)
                {
                    // Create Patient
                    var patient = await _apiClient.CreatePatientAsync(request.PatientDetails);
                    patientId = patient.Id;
                }

                if (patientId == 0)
                {
                    return Json(new { success = false, message = "Patient details are missing." });
                }

                // 2. Resolve Doctor
                int? doctorId = request.DoctorId;
                if (doctorId == 0 && request.DoctorDetails != null)
                {
                    var doctor = await _apiClient.CreateDoctorAsync(request.DoctorDetails);
                    doctorId = doctor.Id;
                }
                if (doctorId <= 0) doctorId = null;

                // 3. Create Order
                var orderReq = new CreateOrderRequest
                {
                    PatientId = patientId,
                    DoctorId = doctorId,
                    TestIds = request.TestIds ?? new List<int>(),
                    PackageIds = request.PackageIds ?? new List<int>(),
                    DiscountAmount = request.DiscountAmount,
                    PaidAmount = request.PaidAmount,
                    PaymentMode = string.IsNullOrEmpty(request.PaymentMode) ? "Cash" : request.PaymentMode,
                    TransactionReference = request.TransactionReference,
                    Remarks = request.Remarks,
                    CustomRates = request.CustomRates
                };

                var order = await _apiClient.CreateOrderAsync(orderReq);

                // 4. Auto-Collect and Receive Samples so they are immediately active for report entry!
                try
                {
                    // Re-fetch order with full samples list to get sample IDs
                    var fullOrder = await _apiClient.GetOrderByIdAsync(order.Id);
                    foreach (var sample in fullOrder.Samples)
                    {
                        var barcode = string.IsNullOrEmpty(sample.Barcode) ? $"BAR-{sample.Id}" : sample.Barcode;
                        // Transition to Collected
                        await _apiClient.CollectSampleAsync(sample.Id, barcode, userId);
                        // Transition to Received
                        await _apiClient.ReceiveSampleAsync(sample.Id, userId);
                    }
                }
                catch (Exception ex)
                {
                    // Log or capture sample transition error, but do not fail order creation
                    System.Diagnostics.Debug.WriteLine($"Failed to auto-process samples: {ex.Message}");
                }

                return Json(new { success = true, orderId = order.Id, message = "Case booked successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder([FromBody] NewEntryUpdateRequest request)
        {
            try
            {
                // 1. Update Patient details
                if (request.PatientDetails != null && request.PatientDetails.Id > 0)
                {
                    await _apiClient.UpdatePatientAsync(request.PatientDetails.Id, request.PatientDetails);
                }

                // 2. Update Discount
                if (request.InvoiceId > 0)
                {
                    await _apiClient.UpdateInvoiceDiscountAsync(request.InvoiceId, request.DiscountAmount);
                }

                return Json(new { success = true, message = "Details updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetResults(int sampleId)
        {
            try
            {
                var results = await _apiClient.GetResultsBySampleIdAsync(sampleId);
                return Json(new { success = true, data = results });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveResults([FromBody] EnterResultsRequest model)
        {
            try
            {
                await _apiClient.EnterResultsAsync(model);
                return Json(new { success = true, message = "Results saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitResults(int sampleId)
        {
            try
            {
                await _apiClient.SubmitResultsForVerificationAsync(sampleId);
                return Json(new { success = true, message = "Results submitted for pathology verification." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyReport(int reportId, string? remarks)
        {
            try
            {
                var report = await _apiClient.VerifyReportAsync(reportId, remarks);
                return Json(new { success = true, message = "Report verified successfully by pathologist.", report = report });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PublishReport(int reportId)
        {
            try
            {
                var report = await _apiClient.PublishReportAsync(reportId);
                return Json(new { success = true, message = "Report PDF published and generated successfully.", report = report });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchPatients(string term)
        {
            try
            {
                var result = await _apiClient.GetPatientsPagedAsync(1, 20, term);
                var selectItems = result.Items.Select(p => new
                {
                    id = p.Id,
                    text = $"{p.PatientCode} - {p.FullName} ({p.Mobile})"
                });
                return Json(selectItems);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

    public class NewEntrySaveRequest
    {
        public int PatientId { get; set; }
        public PatientDto? PatientDetails { get; set; }
        public int? DoctorId { get; set; }
        public DoctorDto? DoctorDetails { get; set; }
        public List<int>? TestIds { get; set; }
        public List<int>? PackageIds { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string? TransactionReference { get; set; }
        public string? Remarks { get; set; }
        public List<CustomRateItem>? CustomRates { get; set; }
    }

    public class NewEntryUpdateRequest
    {
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public PatientDto? PatientDetails { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}
