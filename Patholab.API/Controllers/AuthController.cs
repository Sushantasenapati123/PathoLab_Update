using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patholab.Application.Interfaces;
using Patholab.Shared.DTOs;
using Patholab.Shared.Models;

namespace Patholab.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _authService.LoginAsync(request, cancellationToken);
                return Ok(ApiResponse<LoginResponse>.CreateSuccess(response, "Login successful."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username)) return Unauthorized();

                await _authService.ChangePasswordAsync(username, request, cancellationToken);
                return Ok(ApiResponse.CreateSuccess("Password changed successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public ActionResult<ApiResponse<string>> RefreshToken()
        {
            // JWT stateless logout / refresh in simple setups can just verify validity
            return Ok(ApiResponse<string>.CreateSuccess("Token is valid."));
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult<ApiResponse> Logout()
        {
            return Ok(ApiResponse.CreateSuccess("Logout successful."));
        }
    }
}
