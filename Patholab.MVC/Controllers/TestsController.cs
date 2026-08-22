using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Patholab.MVC.Services;
using Patholab.Shared.DTOs;

namespace Patholab.MVC.Controllers
{
    [Authorize]
    public class TestsController : Controller
    {
        private readonly PatholabApiClient _apiClient;

        public TestsController(PatholabApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index(string? search = null, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _apiClient.GetTestsPagedAsync(pageNumber, pageSize, search);
                ViewBag.Search = search;
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new Shared.Models.PagedResult<TestDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new TestDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestDto model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns();
                return View(model);
            }

            try
            {
                await _apiClient.CreateTestAsync(model);
                TempData["Success"] = "Diagnostic test created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateDropdowns();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var test = await _apiClient.GetTestByIdAsync(id);
                await PopulateDropdowns();
                return View(test);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TestDto model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns();
                return View(model);
            }

            try
            {
                await _apiClient.UpdateTestAsync(id, model);
                TempData["Success"] = "Test settings updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateDropdowns();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apiClient.DeleteTestAsync(id);
                TempData["Success"] = "Test deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Packages()
        {
            try
            {
                var packages = await _apiClient.GetPackagesAsync();
                return View(packages);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task PopulateDropdowns()
        {
            var depts = await _apiClient.GetDepartmentsAsync();
            var types = await _apiClient.GetSampleTypesAsync();

            ViewBag.Departments = new SelectList(depts, "Id", "DepartmentName");
            ViewBag.SampleTypes = new SelectList(types, "Id", "SampleTypeName");
        }
    }
}
