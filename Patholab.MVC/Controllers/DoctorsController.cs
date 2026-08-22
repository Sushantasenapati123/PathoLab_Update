using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patholab.MVC.Services;
using Patholab.Shared.DTOs;

namespace Patholab.MVC.Controllers
{
    [Authorize]
    public class DoctorsController : Controller
    {
        private readonly PatholabApiClient _apiClient;

        public DoctorsController(PatholabApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index(string? search = null, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _apiClient.GetDoctorsPagedAsync(pageNumber, pageSize, search);
                ViewBag.Search = search;
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new Shared.Models.PagedResult<DoctorDto>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new DoctorDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(DoctorDto model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var result = await _apiClient.CreateDoctorAsync(model);
                TempData["Success"] = "Doctor registered successfully with Code: " + result.DoctorCode;
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
                var doctor = await _apiClient.GetDoctorByIdAsync(id);
                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DoctorDto model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                await _apiClient.UpdateDoctorAsync(id, model);
                TempData["Success"] = "Doctor profile updated successfully.";
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
                await _apiClient.DeleteDoctorAsync(id);
                TempData["Success"] = "Doctor profile deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
