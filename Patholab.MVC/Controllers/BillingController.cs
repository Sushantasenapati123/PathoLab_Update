using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patholab.MVC.Services;
using Patholab.Shared.DTOs;

namespace Patholab.MVC.Controllers
{
    [Authorize]
    public class BillingController : Controller
    {
        private readonly PatholabApiClient _apiClient;

        public BillingController(PatholabApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Invoices(string? search = null, int pageNumber = 1, int pageSize = 15)
        {
            try
            {
                var result = await _apiClient.GetInvoicesPagedAsync(pageNumber, pageSize, search);
                ViewBag.Search = search;
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new Shared.Models.PagedResult<InvoiceDto>());
            }
        }

        public async Task<IActionResult> Payments(string? search = null, int pageNumber = 1, int pageSize = 15)
        {
            try
            {
                var result = await _apiClient.GetPaymentsPagedAsync(pageNumber, pageSize, search);
                ViewBag.Search = search;
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new Shared.Models.PagedResult<PaymentDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Collect(int invoiceId)
        {
            try
            {
                var invoice = await _apiClient.GetInvoiceByIdAsync(invoiceId);
                var model = new CreatePaymentRequest
                {
                    InvoiceId = invoiceId,
                    Amount = invoice.DueAmount
                };
                ViewBag.Invoice = invoice;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Invoices));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Collect(CreatePaymentRequest model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var invoice = await _apiClient.GetInvoiceByIdAsync(model.InvoiceId);
                    ViewBag.Invoice = invoice;
                }
                catch { }
                return View(model);
            }

            try
            {
                var payment = await _apiClient.CollectPaymentAsync(model);
                TempData["Success"] = $"Payment of {payment.Amount:C} received successfully. Receipt No: {payment.PaymentNumber}";
                return RedirectToAction(nameof(Invoices));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                try
                {
                    var invoice = await _apiClient.GetInvoiceByIdAsync(model.InvoiceId);
                    ViewBag.Invoice = invoice;
                }
                catch { }
                return View(model);
            }
        }
    }
}
