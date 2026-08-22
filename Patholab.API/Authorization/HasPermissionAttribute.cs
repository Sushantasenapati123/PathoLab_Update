using Microsoft.AspNetCore.Authorization;

namespace Patholab.API.Authorization
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission) : base(permission)
        {
        }
    }
}
