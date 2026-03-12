using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private const string UserNameAndEmailRequiredCode = "1228";
        private const string ValidationFailedCode = "1101";

        private readonly IIdentityService _identityService;

        public UsersController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]
        [PermissionAuthorize("quotations.view")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _identityService.GetUsersAsync();
            return Ok(users);
        }

        [HttpPut("{userId}")]
        [PermissionAuthorize("employees.edit")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateAdminBasicUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new ApiErrorResponse(UserNameAndEmailRequiredCode, StatusCodes.Status400BadRequest));
            }

            if (string.IsNullOrWhiteSpace(request.NameEn) || string.IsNullOrWhiteSpace(request.NameAr))
            {
                return BadRequest(new ApiErrorResponse(
                    ValidationFailedCode,
                    StatusCodes.Status400BadRequest,
                    new List<string> { "NameEn is required", "NameAr is required" }));
            }

            await _identityService.UpdateUserDetailsAsync(
                userId,
                request.UserName.Trim(),
                request.Email.Trim(),
                request.NameEn.Trim(),
                request.NameAr.Trim(),
                request.BranchId,
                null);

            return Ok();
        }

        [HttpPost("{userId}/activate")]
        [PermissionAuthorize("employees.lock")]
        public async Task<IActionResult> ActivateUser(string userId)
        {
            await _identityService.UnlockUserAsync(userId);
            return Ok();
        }

        [HttpPost("{userId}/deactivate")]
        [PermissionAuthorize("employees.lock")]
        public async Task<IActionResult> DeactivateUser(string userId)
        {
            await _identityService.LockUserAsync(userId);
            return Ok();
        }
    }
}
