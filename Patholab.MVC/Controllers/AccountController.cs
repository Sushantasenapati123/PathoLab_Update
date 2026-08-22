using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patholab.MVC.Services;
using Patholab.Shared.DTOs;

namespace Patholab.MVC.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly PatholabApiClient _apiClient;

        public AccountController(PatholabApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var response = await _apiClient.LoginAsync(model);

                // 1. Store JWT Token in Session
                HttpContext.Session.SetString("JWToken", response.Token);
                HttpContext.Session.SetString("Username", response.Username);
                HttpContext.Session.SetString("UserRole", response.Role);
                HttpContext.Session.SetString("UserFullName", response.FullName);

                // 2. Set up Cookie claims for MVC local authorization
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, response.UserId.ToString()),
                    new Claim(ClaimTypes.Name, response.Username),
                    new Claim(ClaimTypes.Role, response.Role),
                    new Claim("FullName", response.FullName)
                };

                foreach (var perm in response.Permissions)
                {
                    claims.Add(new Claim("Permission", perm));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = response.Expiration
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
