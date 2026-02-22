using Microsoft.AspNetCore.Authorization;

namespace CarGalary.Admin.Api.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public const string PolicyPrefix = "Permission:";

        public PermissionAuthorizeAttribute(string permission)
        {
            Policy = $"{PolicyPrefix}{permission}";
        }
    }
}
