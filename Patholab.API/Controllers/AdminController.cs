using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patholab.API.Authorization;
using Patholab.Application.Common;
using Patholab.Application.Interfaces;
using Patholab.Domain.Entities;
using Patholab.Shared.DTOs;
using Patholab.Shared.Models;

namespace Patholab.API.Controllers
{
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuditLogService _auditLogService;
        private readonly IWebHostEnvironment _env;

        public AdminController(IApplicationDbContext context, IAuditLogService auditLogService, IWebHostEnvironment env)
        {
            _context = context;
            _auditLogService = auditLogService;
            _env = env;
        }

        // ==========================================
        // USERS ENDPOINTS
        // ==========================================

        [HttpGet("api/users")]
        [HasPermission("ADMIN_USERS")]
        public async Task<ActionResult<ApiResponse<PagedResult<UserDto>>>> GetUsers(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? search = null, 
            CancellationToken cancellationToken = default)
        {
            var query = _context.Users.Include(u => u.Role).Where(u => !u.DeletedFlag);

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                query = query.Where(u => u.Username.ToLower().Contains(s) || u.FullName.ToLower().Contains(s));
            }

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    FullName = u.FullName,
                    Email = u.Email,
                    Mobile = u.Mobile,
                    RoleId = u.RoleId,
                    RoleName = u.Role.RoleName,
                    IsActive = u.IsActive,
                    LastLoginDate = u.LastLoginDate
                })
                .ToListAsync(cancellationToken);

            return Ok(ApiResponse<PagedResult<UserDto>>.CreateSuccess(new PagedResult<UserDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = total,
                TotalPages = (int)Math.Ceiling((double)total / pageSize)
            }));
        }

        [HttpPost("api/users")]
        [HasPermission("ADMIN_USERS")]
        public async Task<ActionResult<ApiResponse<UserDto>>> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var exists = await _context.Users.AnyAsync(u => u.Username == request.Username && !u.DeletedFlag, cancellationToken);
                if (exists) throw new Exception("Username is already taken.");

                var user = new User
                {
                    Username = request.Username,
                    FullName = request.FullName,
                    Email = request.Email,
                    Mobile = request.Mobile,
                    PasswordHash = PasswordHasher.HashPassword(request.Password),
                    RoleId = request.RoleId,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);

                return Ok(ApiResponse<UserDto>.CreateSuccess(new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    FullName = user.FullName,
                    Email = user.Email,
                    Mobile = user.Mobile,
                    RoleId = user.RoleId,
                    IsActive = user.IsActive
                }, "User created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        // ==========================================
        // ROLES ENDPOINTS
        // ==========================================

        [HttpGet("api/roles")]
        public async Task<ActionResult<ApiResponse<List<RoleDto>>>> GetRoles(CancellationToken cancellationToken)
        {
            var roles = await _context.Roles
                .Where(r => r.IsActive && !r.DeletedFlag)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    RoleName = r.RoleName,
                    Description = r.Description,
                    IsActive = r.IsActive
                })
                .ToListAsync(cancellationToken);

            return Ok(ApiResponse<List<RoleDto>>.CreateSuccess(roles));
        }

        // ==========================================
        // PERMISSIONS ENDPOINTS
        // ==========================================

        [HttpGet("api/permissions")]
        public async Task<ActionResult<ApiResponse<List<PermissionDto>>>> GetPermissions(CancellationToken cancellationToken)
        {
            var permissions = await _context.Permissions
                .Where(p => p.IsActive)
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    PermissionCode = p.PermissionCode,
                    PermissionName = p.PermissionName,
                    ModuleName = p.ModuleName,
                    Description = p.Description,
                    IsActive = p.IsActive
                })
                .ToListAsync(cancellationToken);

            return Ok(ApiResponse<List<PermissionDto>>.CreateSuccess(permissions));
        }

        // ==========================================
        // AUDIT LOGS ENDPOINTS
        // ==========================================

        [HttpGet("api/auditlogs")]
        [HasPermission("ADMIN_USERS")]
        public async Task<ActionResult<ApiResponse<PagedResult<AuditLogDto>>>> GetAuditLogs(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? search = null, 
            CancellationToken cancellationToken = default)
        {
            var result = await _auditLogService.GetAuditLogsPagedAsync(pageNumber, pageSize, search, cancellationToken);
            return Ok(ApiResponse<PagedResult<AuditLogDto>>.CreateSuccess(result));
        }

        // ==========================================
        // SYSTEM LOGS ENDPOINTS
        // ==========================================

        [HttpGet("api/admin/todaylogs")]
        [HasPermission("ADMIN_USERS")]
        public async Task<ActionResult<ApiResponse<string>>> GetTodayLogs(CancellationToken cancellationToken)
        {
            try
            {
                var logFileName = $"log-{DateTime.Now:yyyyMMdd}.txt";
                var logFilePath = Path.Combine(_env.ContentRootPath, "Logs", logFileName);

                if (!System.IO.File.Exists(logFilePath))
                {
                    return Ok(ApiResponse<string>.CreateSuccess(string.Empty, "No log file found for today."));
                }

                using (var fileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fileStream))
                {
                    var content = await reader.ReadToEndAsync();
                    return Ok(ApiResponse<string>.CreateSuccess(content));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError($"Failed to read log file: {ex.Message}"));
            }
        }
    }
}
