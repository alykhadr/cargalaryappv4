using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CarGalary.Admin.Api.Security
{
    public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User?.Identity?.IsAuthenticated != true)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var required = requirement.Permission.Trim();
            var hasPermission = context.User.Claims.Any(claim =>
                string.Equals(claim.Type, "permission", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(claim.Value?.Trim(), required, StringComparison.OrdinalIgnoreCase));

            if (hasPermission)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
