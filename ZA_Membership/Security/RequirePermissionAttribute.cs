using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ZA_Membership.Security
{
    /// <summary>
    /// Attribute to enforce permission-based authorization on controller actions.
    /// </summary>
    public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _permission;

        /// <summary>
        /// Initializes a new instance of the RequirePermissionAttribute class with the specified permission.
        /// </summary>
        /// <param name="permission"></param>
        public RequirePermissionAttribute(string permission)
        {
            _permission = permission;
        }
        /// <summary>
        /// Called to perform authorization check.
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var permissions = user.FindAll("permission").Select(c => c.Value).ToList();

            if (!permissions.Contains(_permission))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}