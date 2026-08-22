using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patholab.MVC.Services;
using Patholab.Shared.DTOs;

namespace Patholab.MVC.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private readonly PatholabApiClient _apiClient;

        public PatientsController(PatholabApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index(string? search = null, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _apiClient.GetPatientsPagedAsync(pageNumber, pageSize, search);
                ViewBag.Search = search;
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new Shared.Models.PagedResult<PatientDto>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var patient = await _apiClient.GetPatientByIdAsync(id);
                var history = await _apiClient.GetPatientHistoryAsync(id, 1, 50); // Get recent order history
                ViewBag.History = history.Items;
                return View(patient);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new PatientDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientDto model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var result = await _apiClient.CreatePatientAsync(model);
                TempData["Success"] = "Patient registered successfully with Code: " + result.PatientCode;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var patient = await _apiClient.GetPatientByIdAsync(id);
                return View(patient);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PatientDto model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                await _apiClient.UpdatePatientAsync(id, model);
                TempData["Success"] = "Patient profile updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apiClient.DeletePatientAsync(id);
                TempData["Success"] = "Patient record deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
