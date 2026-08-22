using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Patholab.MVC.Services;
using Patholab.Shared.DTOs;

namespace Patholab.MVC.Controllers
{
    [Authorize(Roles = "Super Admin,Lab Admin")]
    public class AdminController : Controller
    {
        private readonly PatholabApiClient _apiClient;

        public AdminController(PatholabApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Users(string? search = null, int pageNumber = 1, int pageSize = 15)
        {
            try
            {
                var result = await _apiClient.GetUsersPagedAsync(pageNumber, pageSize, search);
                var roles = await _apiClient.GetRolesAsync();
                
                ViewBag.Roles = new SelectList(roles, "Id", "RoleName");
                ViewBag.Search = search;
                
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new Shared.Models.PagedResult<UserDto>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid user registration details.";
                return RedirectToAction(nameof(Users));
            }

            try
            {
                await _apiClient.CreateUserAsync(request);
                TempData["Success"] = "User account created successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> AuditLogs(string? search = null, int pageNumber = 1, int pageSize = 15)
        {
            try
            {
                var result = await _apiClient.GetAuditLogsPagedAsync(pageNumber, pageSize, search);
                ViewBag.Search = search;
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new Shared.Models.PagedResult<AuditLogDto>());
            }
        }

        public async Task<IActionResult> TodayLogs()
        {
            try
            {
                var logs = await _apiClient.GetTodayLogsAsync();
                return View("TodayLogs", (object)logs);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Could not retrieve logs: {ex.Message}";
                return View("TodayLogs", (object)string.Empty);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTodayLogsJson()
        {
            try
            {
                var logs = await _apiClient.GetTodayLogsAsync();
                return Json(new { success = true, logs = logs });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
