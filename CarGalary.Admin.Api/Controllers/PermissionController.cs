using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/permissions")]
    public class PermissionController : ControllerBase
    {
        private readonly IIdentityService _identity;

        public PermissionController(IIdentityService identity)
        {
            _identity = identity;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var permissions = await _identity.GetPermissionsAsync();
            return Ok(permissions);
        }

        [HttpGet("roles/{roleId}")]
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
        public async Task<IActionResult> AddPermissionToRole(string roleId, [FromBody] AddRolePermissionRequest request)
        {
            var permission = request?.Permission;
            if (string.IsNullOrWhiteSpace(permission))
            {
                return BadRequest(new ApiErrorResponse("Permission is required"));
            }

            await _identity.AssignPermissionToRoleAsync(roleId, permission.Trim());
            return Ok();
        }

        [HttpDelete("roles/{roleId}/{permission}")]
        public async Task<IActionResult> RemovePermissionFromRole(string roleId, string permission)
        {
            if (string.IsNullOrWhiteSpace(permission))
            {
                return BadRequest(new ApiErrorResponse("Permission is required"));
            }

            await _identity.RemovePermissionFromRoleAsync(roleId, permission.Trim());
            return Ok();
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserPermissions(string userId)
        {
            var permissions = await _identity.GetUserPermissionsAsync(userId);
            return Ok(permissions);
        }
    }
}
