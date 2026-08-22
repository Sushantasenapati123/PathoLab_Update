using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Patholab.Application.Common;
using Patholab.Application.Interfaces;
using Patholab.Shared.DTOs;

namespace Patholab.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IApplicationDbContext _context;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthService(IApplicationDbContext context, ITokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.IsActive && !u.DeletedFlag, cancellationToken);

            if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password.");
            }

            var permissions = user.Role.RolePermissions
                .Where(rp => rp.Permission.IsActive && !rp.Permission.DeletedFlag)
                .Select(rp => rp.Permission.PermissionCode)
                .ToList();

            var token = _tokenGenerator.GenerateToken(user.Id, user.Username, user.Role.RoleName, permissions, out var expiration);

            user.LastLoginDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return new LoginResponse
            {
                Token = token,
                Expiration = expiration,
                UserId = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role.RoleName,
                Permissions = permissions
            };
        }

        public async Task ChangePasswordAsync(string username, ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive && !u.DeletedFlag, cancellationToken);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (!PasswordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                throw new Exception("Current password is incorrect.");
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                throw new Exception("New passwords do not match.");
            }

            user.PasswordHash = PasswordHasher.HashPassword(request.NewPassword);
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = username;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
