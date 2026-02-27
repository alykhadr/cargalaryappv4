using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CarGalary.Admin.Api.Security
{
    public sealed class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            // Force JWT bearer for plain [Authorize] so Identity cookie defaults
            // do not trigger login redirects for API endpoints.
            var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            return Task.FromResult(policy);
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return _fallbackPolicyProvider.GetFallbackPolicyAsync();
        }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(PermissionAuthorizeAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return _fallbackPolicyProvider.GetPolicyAsync(policyName);
            }

            var permission = policyName[PermissionAuthorizeAttribute.PolicyPrefix.Length..];
            var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .AddRequirements(new PermissionRequirement(permission))
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }
    }
}
