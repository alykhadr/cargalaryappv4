using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/permissions")]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly IIdentityService _identity;

        public PermissionController(IIdentityService identity)
        {
            _identity = identity;
        }

        [HttpGet]
        [PermissionAuthorize("permissions.view")]
        public async Task<IActionResult> GetAll()
        {
            var permissions = await _identity.GetPermissionsAsync();
            return Ok(permissions);
        }

        [HttpGet("roles/{roleId}")]
        [PermissionAuthorize("permissions.view")]
        public async Task<IActionResult> GetRolePermissions(string roleId)
        {
            var role = await _identity.GetRoleByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new ApiErrorResponse("Role not found", StatusCodes.Status404NotFound));
            }

            var permissions = await _identity.GetRolePermissionsAsync(roleId);
            return Ok(permissions);
        }

        [HttpPost("roles/{roleId}")]
        [PermissionAuthorize("permissions.create")]
        public async Task<IActionResult> AddPermissionToRole(string roleId, [FromBody] AddRolePermissionRequest request)
        {
            var page = request?.Page?.Trim();
            var action = request?.Action?.Trim();

            if (string.IsNullOrWhiteSpace(page))
            {
                return BadRequest(new ApiErrorResponse("Page is required"));
            }

            if (string.IsNullOrWhiteSpace(action))
            {
                return BadRequest(new ApiErrorResponse("Action is required"));
            }

            var permission = BuildPermission(page, action);
            await _identity.AssignPermissionToRoleAsync(roleId, permission);
            return Ok();
        }

        [HttpDelete("roles/{roleId}/{permission}")]
        [PermissionAuthorize("permissions.delete")]
        public async Task<IActionResult> RemovePermissionFromRole(string roleId, string permission)
        {
            if (string.IsNullOrWhiteSpace(permission))
            {
                return BadRequest(new ApiErrorResponse("Permission is required"));
            }

            await _identity.RemovePermissionFromRoleAsync(roleId, permission.Trim());
            return Ok();
        }

        [HttpGet("employees/{userId}")]
        [PermissionAuthorize("permissions.view")]
        public async Task<IActionResult> GetEmployeePermissions(string userId)
        {
            var permissions = await _identity.GetUserPermissionsAsync(userId);
            return Ok(permissions);
        }

        private static string BuildPermission(string page, string action)
        {
            return $"{page.Trim()}.{action.Trim()}";
        }
    }
}
