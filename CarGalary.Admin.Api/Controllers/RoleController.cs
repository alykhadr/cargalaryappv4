using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IIdentityService _identity;

        public RoleController(IIdentityService identity)
        {
            _identity = identity;
        }

        [HttpGet]
        [PermissionAuthorize("roles.view")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _identity.GetRolesAsync());
        }

        [HttpGet("{roleId}")]
        [PermissionAuthorize("roles.view")]
        public async Task<IActionResult> GetById(string roleId)
        {
            var role = await _identity.GetRoleByIdAsync(roleId);
            return role == null ? NotFound(new ApiErrorResponse("Role not found", StatusCodes.Status404NotFound)) : Ok(role);
        }

        [HttpGet("{roleId}/users")]
        [PermissionAuthorize("roles.view")]
        public async Task<IActionResult> GetUsersByRole(string roleId)
        {
            var role = await _identity.GetRoleByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new ApiErrorResponse("Role not found", StatusCodes.Status404NotFound));
            }

            var users = await _identity.GetUsersByRoleIdAsync(roleId);
            return Ok(users);
        }

        [HttpPost]
        [PermissionAuthorize("roles.create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateRoleRequest request,
            [FromServices] IValidator<CreateRoleRequest> validator)
        {
            var validation = validator.Validate(request);
            if (!validation.IsValid)
            {
                return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, validation.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var roleName = request.Name.Trim();
            if (await _identity.RoleExistsAsync(roleName))
            {
                return BadRequest(new ApiErrorResponse($"Role '{roleName}' already exists"));
            }

            request.Name = roleName;
            var role = await _identity.CreateRoleAsync(request);
            return Ok(role);
        }

        [HttpPut("{roleId}")]
        [PermissionAuthorize("roles.edit")]
        public async Task<IActionResult> Update(
            string roleId,
            [FromBody] UpdateRoleRequest request,
            [FromServices] IValidator<UpdateRoleRequest> validator)
        {
            var existing = await _identity.GetRoleByIdAsync(roleId);
            if (existing == null)
            {
                return NotFound(new ApiErrorResponse("Role not found", StatusCodes.Status404NotFound));
            }

            var validation = validator.Validate(request);
            if (!validation.IsValid)
            {
                return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, validation.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var roleName = request.Name.Trim();
            if (!string.Equals(existing.Name, roleName, StringComparison.OrdinalIgnoreCase)
                && await _identity.RoleExistsAsync(roleName))
            {
                return BadRequest(new ApiErrorResponse($"Role '{roleName}' already exists"));
            }

            request.Name = roleName;
            var updated = await _identity.UpdateRoleAsync(roleId, request);
            return updated ? Ok() : NotFound(new ApiErrorResponse("Role not found", StatusCodes.Status404NotFound));
        }

        [HttpDelete("{roleId}")]
        [PermissionAuthorize("roles.delete")]
        public async Task<IActionResult> Delete(string roleId)
        {
            var deleted = await _identity.DeleteRoleAsync(roleId);
            return deleted ? Ok() : NotFound(new ApiErrorResponse("Role not found", StatusCodes.Status404NotFound));
        }

        [HttpDelete("bulk")]
        [PermissionAuthorize("roles.delete")]
        public async Task<IActionResult> DeleteBulk([FromBody] List<string> roleIds)
        {
            if (roleIds == null || roleIds.Count == 0)
            {
                return BadRequest(new ApiErrorResponse("roleIds is required"));
            }

            var normalizedIds = roleIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id => id.Trim())
                .Distinct()
                .ToList();

            if (normalizedIds.Count == 0)
            {
                return BadRequest(new ApiErrorResponse("roleIds must contain valid values"));
            }

            var notFoundIds = new List<string>();

            foreach (var roleId in normalizedIds)
            {
                var deleted = await _identity.DeleteRoleAsync(roleId);
                if (!deleted)
                {
                    notFoundIds.Add(roleId);
                }
            }

            if (notFoundIds.Count > 0)
            {
                return NotFound(new ApiErrorResponse("Some roles were not found", StatusCodes.Status404NotFound, notFoundIds));
            }

            return Ok();
        }
    }
}
