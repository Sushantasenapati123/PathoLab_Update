using System;
using System.Collections.Generic;

namespace Patholab.Shared.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginDate { get; set; }
    }

    public class CreateUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }

    public class RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }

    public class PermissionDto
    {
        public int Id { get; set; }
        public string PermissionCode { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateRolePermissionsRequest
    {
        public int RoleId { get; set; }
        public List<int> PermissionIds { get; set; } = new List<int>();
    }

    public class AuditLogDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
