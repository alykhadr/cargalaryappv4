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
                var raw = _httpContextAccessor.HttpContext?.User?.FindFirstValue("branch_id");
                if (int.TryParse(raw, out var branchId))
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
