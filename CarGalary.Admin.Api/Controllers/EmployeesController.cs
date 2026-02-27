using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IIdentityService _identity;
        private readonly IEmployeeService _employeeService;
        private readonly IValidator<RegisterRequest> _registerValidator;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeesController(
            IIdentityService identity,
            IEmployeeService employeeService,
            IValidator<RegisterRequest> registerValidator,
            UserManager<ApplicationUser> userManager)
        {
            _identity = identity;
            _employeeService = employeeService;
            _registerValidator = registerValidator;
            _userManager = userManager;
        }

        [PermissionAuthorize("employees.create")]
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromForm] RegisterRequest request)
        {
            var validator = _registerValidator.Validate(request);
            if (!validator.IsValid)
            {
                var errors = validator.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
            }

            var normalizedEmail = request.Email?.ToUpper().Trim() ?? string.Empty;
            var normalizedUserName = request.UserName?.Trim() ?? string.Empty;
            var password = request.Password ?? string.Empty;

            var emailExist = await _identity.GetUserByEmailAsync(normalizedEmail);
            if (!string.IsNullOrWhiteSpace(emailExist))
            {
                return BadRequest(new ApiErrorResponse($"email : {request.Email} already exist"));
            }

            string? profileImageUrl = null;
            if (request.ProfileImage != null)
            {
                profileImageUrl = await SaveProfileImageAsync(request.ProfileImage);
            }

            var user = await _identity.CreateUserAsync(
                normalizedUserName,
                normalizedEmail,
                password,
                request.FirstName?.Trim(),
                request.LastName?.Trim(),
                request.BranchId,
                profileImageUrl);

            if (string.IsNullOrWhiteSpace(user.Id))
            {
                return BadRequest(new ApiErrorResponse("Invalid created user id"));
            }

            try
            {
                if (!Guid.TryParse(user.Id, out var createdUserId))
                {
                    await _identity.DeleteUserAsync(user.Id);
                    return BadRequest(new ApiErrorResponse("Invalid created user id"));
                }

                await _employeeService.CreateEmployeeAsync(request, createdUserId);
            }
            catch (Exception ex)
            {
                await _identity.DeleteUserAsync(user.Id);
                return BadRequest(new ApiErrorResponse(ex.Message));
            }

            var userRoles = request.Roles ?? new List<string>();
            foreach (var role in userRoles)
            {
                await _identity.AssignRoleAsync(user.Id!, role);
            }

            return Ok(user);
        }

        [PermissionAuthorize("employees.view")]
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeService.GetEmployeesAsync();

            return Ok(employees);
        }

        [PermissionAuthorize("employees.view")]
        [HttpGet("branch/{branchId}")]
        public async Task<IActionResult> GetEmployeesByBranch(int branchId)
        {
            var employees = await _employeeService.GetEmployeesByBranchAsync(branchId);
            return Ok(employees);
        }

        [PermissionAuthorize("employees.view")]
        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> GetEmployeesByDepartment(int departmentId)
        {
            var employees = await _employeeService.GetEmployeesByDepartmentAsync(departmentId);
            return Ok(employees);
        }

        [PermissionAuthorize("employees.edit")]
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateEmployee(string userId, [FromForm] UpdateAdminUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new ApiErrorResponse("Username and email are required"));
            }

            string? profileImageUrl = null;
            if (request.ProfileImage != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null && !string.IsNullOrWhiteSpace(user.ProfileImageUrl))
                {
                    DeleteProfileImage(user.ProfileImageUrl);
                }
                profileImageUrl = await SaveProfileImageAsync(request.ProfileImage);
            }

            await _identity.UpdateUserDetailsAsync(
                userId,
                request.UserName,
                request.Email,
                request.FirstName,
                request.LastName,
                request.BranchId,
                profileImageUrl
            );

            if (Guid.TryParse(userId, out var parsedUserId))
            {
                await _employeeService.UpdateEmployeeAsync(parsedUserId, request);
            }

            return Ok();
        }

        [PermissionAuthorize("employees.edit")]
        [HttpPost("{userId}/change-password")]
        public async Task<IActionResult> ChangeEmployeePassword(string userId, [FromBody] ChangeUserPasswordByAdminRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest(new ApiErrorResponse("New password is required"));
            }
            if (request.NewPassword.Length < 6)
            {
                return BadRequest(new ApiErrorResponse("New password must be at least 6 characters"));
            }

            await _identity.ChangeUserPasswordByAdminAsync(userId, request.NewPassword);
            return Ok("Password changed successfully");
        }

        [PermissionAuthorize("employees.delete")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteEmployee(string userId)
        {
            if (Guid.TryParse(userId, out var parsedUserId))
            {
                await _employeeService.DeleteEmployeeAsync(parsedUserId);
            }

            var result = await _identity.DeleteUserAsync(userId);

            if (!result)
                return NotFound(new ApiErrorResponse("User not found", StatusCodes.Status404NotFound));

            return NoContent();
        }

        [PermissionAuthorize("employees.delete")]
        [HttpPost("bulk-delete")]
        public async Task<IActionResult> BulkDeleteEmployees([FromBody] BulkDeleteUsersRequest request)
        {
            if (request.UserIds == null || !request.UserIds.Any())
            {
                return BadRequest(new ApiErrorResponse("User IDs are required"));
            }

            var deletedCount = 0;
            var failedIds = new List<string>();

            foreach (var userId in request.UserIds)
            {
                if (Guid.TryParse(userId, out var parsedUserId))
                {
                    await _employeeService.DeleteEmployeeAsync(parsedUserId);
                }

                var result = await _identity.DeleteUserAsync(userId);
                if (result)
                {
                    deletedCount++;
                }
                else
                {
                    failedIds.Add(userId);
                }
            }

            return Ok(new { deletedCount, failedIds });
        }

        [PermissionAuthorize("employees.lock")]
        [HttpPost("{userId}/lock")]
        public async Task<IActionResult> LockEmployee(string userId)
        {
            await _identity.LockUserAsync(userId);
            return Ok("User locked");
        }

        [PermissionAuthorize("employees.lock")]
        [HttpPost("{userId}/unlock")]
        public async Task<IActionResult> UnlockEmployee(string userId)
        {
            await _identity.UnlockUserAsync(userId);
            return Ok("User unlocked");
        }

        [PermissionAuthorize("employees.view")]
        [HttpGet("{userId}/roles")]
        public async Task<IActionResult> GetEmployeeRoles(string userId)
        {
            var roles = await _identity.GetUserRolesAsync(userId);
            return Ok(roles);
        }

        [PermissionAuthorize("employees.view")]
        [HttpGet("{userId}/permissions")]
        public async Task<IActionResult> GetEmployeePermissions(string userId)
        {
            var permissions = await _identity.GetUserPermissionsAsync(userId);
            return Ok(permissions);
        }

        [PermissionAuthorize("employees.roles")]
        [HttpPost("{userId}/roles/{role}")]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            await _identity.AssignRoleAsync(userId, role);
            return Ok("Role assigned");
        }

        [PermissionAuthorize("employees.roles")]
        [HttpDelete("{userId}/roles/{role}")]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            await _identity.RemoveRoleAsync(userId, role);
            return Ok("Role removed");
        }

        private async Task<string> SaveProfileImageAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/profiles/{uniqueFileName}";
        }

        private void DeleteProfileImage(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) return;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
