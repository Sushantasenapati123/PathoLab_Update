using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patholab.Domain.Enums;
using Patholab.MVC.Services;
using Patholab.Shared.DTOs;

namespace Patholab.MVC.Controllers
{
    [Authorize]
    public class HomeCollectionController : Controller
    {
        private readonly PatholabApiClient _apiClient;

        public HomeCollectionController(PatholabApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index(string? search = null, int pageNumber = 1, int pageSize = 15)
        {
            try
            {
                var result = await _apiClient.GetHomeCollectionsPagedAsync(pageNumber, pageSize, search);
                ViewBag.Search = search;
                
                // Load active agents for assignments dropdown
                var usersResult = await _apiClient.GetUsersPagedAsync(1, 100, null);
                ViewBag.Agents = usersResult.Items; // In clinical setting, filter by Role == "Collection Agent" or display all
                
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new Shared.Models.PagedResult<HomeCollectionDto>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestHomeCollectionRequest model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid home collection schedule details.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _apiClient.RequestHomeCollectionAsync(model);
                TempData["Success"] = "Home collection schedule requested successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Assign(int hcId, int agentId)
        {
            try
            {
                await _apiClient.AssignHomeCollectionAgentAsync(hcId, agentId);
                TempData["Success"] = "Home collection agent assigned successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int hcId, HomeCollectionStatus status, string? remarks)
        {
            try
            {
                await _apiClient.UpdateHomeCollectionStatusAsync(hcId, status, remarks);
                TempData["Success"] = $"Collection visit status updated to: {status}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
