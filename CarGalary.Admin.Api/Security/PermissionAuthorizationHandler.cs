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
            var acceptedPermissions = GetAcceptedPermissions(required);
            var hasPermission = context.User.Claims.Any(claim =>
                string.Equals(claim.Type, "permission", StringComparison.OrdinalIgnoreCase) &&
                acceptedPermissions.Contains(claim.Value?.Trim() ?? string.Empty, StringComparer.OrdinalIgnoreCase));

            if (hasPermission)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private static IEnumerable<string> GetAcceptedPermissions(string required)
        {
            yield return required;

            // Backward compatibility after renaming users.* permissions to employees.*
            if (required.StartsWith("employees.", StringComparison.OrdinalIgnoreCase))
            {
                yield return "users." + required["employees.".Length..];
            }
            else if (required.StartsWith("users.", StringComparison.OrdinalIgnoreCase))
            {
                yield return "employees." + required["users.".Length..];
            }
        }
    }
}
