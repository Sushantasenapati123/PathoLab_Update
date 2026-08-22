using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Patholab.API.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // Extract permission claims from JWT
            var permissions = context.User.FindAll("Permission").Select(c => c.Value);
            var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

            // Grant access if user has the specific permission or is a Super Admin
            if (permissions.Contains(requirement.Permission) || role == "Super Admin")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
