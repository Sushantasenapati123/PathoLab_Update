using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Patholab.MVC.Services;
using Patholab.Shared.DTOs;

namespace Patholab.MVC.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly PatholabApiClient _apiClient;

        public OrdersController(PatholabApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index(string? search = null, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _apiClient.GetOrdersPagedAsync(pageNumber, pageSize, search);
                ViewBag.Search = search;
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new Shared.Models.PagedResult<OrderDto>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await _apiClient.GetOrderByIdAsync(id);
                // Try fetching invoice details if billing exists
                try
                {
                    var invoice = await _apiClient.GetInvoiceByIdAsync(id); // InvoiceId corresponds to OrderId in standard workflow
                    ViewBag.Invoice = invoice;
                }
                catch
                {
                    ViewBag.Invoice = null;
                }
                return View(order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? patientId = null)
        {
            try
            {
                var patientsResult = await _apiClient.GetPatientsPagedAsync(1, 100, null);
                var doctorsResult = await _apiClient.GetDoctorsPagedAsync(1, 100, null);
                var tests = await _apiClient.GetTestsPagedAsync(1, 100, null);
                var packages = await _apiClient.GetPackagesAsync();

                ViewBag.Patients = patientsResult.Items.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.PatientCode} - {p.FirstName} {p.LastName} ({p.Mobile})",
                    Selected = p.Id == patientId
                }).ToList();

                ViewBag.Doctors = doctorsResult.Items.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.DoctorCode} - {d.DoctorName} ({d.Specialization})"
                }).ToList();

                ViewBag.Tests = tests.Items;
                ViewBag.Packages = packages;

                var model = new CreateOrderRequest { PatientId = patientId ?? 0 };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to load order creation settings: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderRequest model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid order submission details.";
                return RedirectToAction(nameof(Create), new { patientId = model.PatientId });
            }

            try
            {
                var order = await _apiClient.CreateOrderAsync(model);
                TempData["Success"] = $"Order {order.OrderNumber} booked successfully! Invoice and receipt generated.";
                return RedirectToAction(nameof(Details), new { id = order.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Create), new { patientId = model.PatientId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id, string remarks)
        {
            if (string.IsNullOrEmpty(remarks))
            {
                TempData["Error"] = "Cancellation remarks are required.";
                return RedirectToAction(nameof(Details), new { id });
            }

            try
            {
                await _apiClient.CancelOrderAsync(id, remarks);
                TempData["Success"] = "Order cancelled successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
