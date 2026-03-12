using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private const string BranchHeaderName = "X-Branch-Id";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId =>
            _httpContextAccessor.HttpContext?.User?
                .FindFirstValue(ClaimTypes.NameIdentifier);

        public string? UserName =>
            _httpContextAccessor.HttpContext?.User?
                .Identity?.Name;

        public string? Email =>
            _httpContextAccessor.HttpContext?.User?
                .FindFirstValue(ClaimTypes.Email);

        public int? BranchId
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext?.User == null)
                {
                    return null;
                }

                var user = httpContext.User;
                var canSwitchBranch = user.IsInRole("Admin") || user.IsInRole("Manager");

                if (canSwitchBranch &&
                    httpContext.Request.Headers.TryGetValue(BranchHeaderName, out var branchHeaderValues) &&
                    int.TryParse(branchHeaderValues.FirstOrDefault(), out var selectedBranchId) &&
                    selectedBranchId > 0)
                {
                    return selectedBranchId;
                }

                var raw = user.FindFirstValue("branch_id");
                if (int.TryParse(raw, out var branchId) && branchId > 0)
                {
                    return branchId;
                }

                return null;
            }
        }

        public bool IsInRole(string roleName)
        {
            return _httpContextAccessor.HttpContext?.User?.IsInRole(roleName) == true;
        }
    }
}
