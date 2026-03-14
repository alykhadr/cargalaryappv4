using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CarGalary.Admin.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private const string ValidationFailedCode = "1101";
        private const string UserNotFoundCode = "1227";
        private const string UserIdsRequiredCode = "1226";
        private const string UserNameAndEmailRequiredCode = "1228";
        private const string NewPasswordRequiredCode = "1105";
        private const string NewPasswordMinLengthCode = "1106";
        private const string InvalidCreatedUserIdCode = "1217";
        private const string EmailAlreadyExistsCode = "1320";
        private const string EmployeeOperationFailedCode = "1331";

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
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            var trimmedEmail = request.Email?.Trim() ?? string.Empty;
            var normalizedEmail = _userManager.NormalizeEmail(trimmedEmail) ?? string.Empty;
            var trimmedUserName = request.UserName?.Trim() ?? string.Empty;
            var normalizedUserName = _userManager.NormalizeName(trimmedUserName) ?? string.Empty;
            var password = request.Password ?? string.Empty;

            var emailExists = await _userManager.Users
                .AsNoTracking()
                .AnyAsync(u => u.NormalizedEmail == normalizedEmail);
            if (emailExists)
            {
                return BadRequest(new ApiErrorResponse(EmailAlreadyExistsCode, StatusCodes.Status400BadRequest));
            }

            var userNameExists = await _userManager.Users
                .AsNoTracking()
                .AnyAsync(u => u.NormalizedUserName == normalizedUserName);
            if (userNameExists)
            {
                return BadRequest(BuildLocalizedValidationError($"Username '{trimmedUserName}' already exists"));
            }

            string? profileImageUrl = null;
            if (request.ProfileImage != null)
            {
                profileImageUrl = await SaveProfileImageAsync(request.ProfileImage);
            }

            UserDto user;
            try
            {
                user = await _identity.CreateUserAsync(
                    trimmedUserName,
                    trimmedEmail,
                    password,
                    request.NameEn?.Trim(),
                    request.NameAr?.Trim(),
                    request.BranchId,
                    profileImageUrl);
            }
            catch (Exception ex) when (ex.Message.Contains("Username", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(BuildLocalizedValidationError(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(BuildLocalizedValidationError(ex.Message));
            }

            if (string.IsNullOrWhiteSpace(user.Id))
            {
                return BadRequest(new ApiErrorResponse(InvalidCreatedUserIdCode, StatusCodes.Status400BadRequest));
            }

            try
            {
                if (!Guid.TryParse(user.Id, out var createdUserId))
                {
                    await _identity.DeleteUserAsync(user.Id);
                    return BadRequest(new ApiErrorResponse(InvalidCreatedUserIdCode, StatusCodes.Status400BadRequest));
                }

                await _employeeService.CreateEmployeeAsync(request, createdUserId);
            }
            catch (Exception ex)
            {
                await _identity.DeleteUserAsync(user.Id);
                return BadRequest(BuildLocalizedValidationError(ex.Message));
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
            try
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
                    request.NameEn?.Trim(),
                    request.NameAr?.Trim(),
                    request.BranchId,
                    profileImageUrl
                );

                if (Guid.TryParse(userId, out var parsedUserId))
                {
                    await _employeeService.UpdateEmployeeAsync(parsedUserId, request);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(BuildLocalizedValidationError(ex.Message));
            }
        }

        [PermissionAuthorize("employees.edit")]
        [HttpPost("{userId}/change-password")]
        public async Task<IActionResult> ChangeEmployeePassword(string userId, [FromBody] ChangeUserPasswordByAdminRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.NewPassword))
                {
                    return BadRequest(new ApiErrorResponse(NewPasswordRequiredCode, StatusCodes.Status400BadRequest));
                }
                if (request.NewPassword.Length < 6)
                {
                    return BadRequest(new ApiErrorResponse(NewPasswordMinLengthCode, StatusCodes.Status400BadRequest));
                }

                await _identity.ChangeUserPasswordByAdminAsync(userId, request.NewPassword);
                return Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(BuildLocalizedValidationError(ex.Message));
            }
        }

        [PermissionAuthorize("employees.delete")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteEmployee(string userId)
        {
            try
            {
                if (Guid.TryParse(userId, out var parsedUserId))
                {
                    await _employeeService.DeleteEmployeeAsync(parsedUserId);
                }

                var result = await _identity.DeleteUserAsync(userId);

                if (!result)
                    return NotFound(new ApiErrorResponse(UserNotFoundCode, StatusCodes.Status404NotFound));

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(BuildLocalizedValidationError(ex.Message));
            }
        }

        [PermissionAuthorize("employees.delete")]
        [HttpPost("bulk-delete")]
        public async Task<IActionResult> BulkDeleteEmployees([FromBody] BulkDeleteUsersRequest request)
        {
            if (request.UserIds == null || !request.UserIds.Any())
            {
                return BadRequest(new ApiErrorResponse(UserIdsRequiredCode, StatusCodes.Status400BadRequest));
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

        private static ApiErrorResponse BuildLocalizedValidationError(string message)
        {
            var safeMessage = string.IsNullOrWhiteSpace(message) ? "Validation failed." : message.Trim();
            var messageEn = safeMessage;
            var messageAr = safeMessage;

            var usernameMatch = Regex.Match(safeMessage, "^Username\\s+'(.+)'\\s+already exists$", RegexOptions.IgnoreCase);
            if (usernameMatch.Success)
            {
                var userName = usernameMatch.Groups[1].Value;
                messageEn = $"Username '{userName}' already exists";
                messageAr = $"اسم المستخدم '{userName}' مستخدم بالفعل.";
            }
            else
            {
                var emailMatch = Regex.Match(safeMessage, "^Email\\s+'(.+)'\\s+already exists$", RegexOptions.IgnoreCase);
                if (emailMatch.Success)
                {
                    var email = emailMatch.Groups[1].Value;
                    messageEn = $"Email '{email}' already exists";
                    messageAr = $"البريد الإلكتروني '{email}' مستخدم بالفعل.";
                }
                else
                {
                    (messageEn, messageAr) = safeMessage switch
                    {
                        "Department is required" => ("Department is required.", "القسم مطلوب."),
                        "Department not found" => ("Department not found.", "القسم غير موجود."),
                        "National ID is required" => ("National ID is required.", "رقم الهوية مطلوب."),
                        "Invalid Nationality" => ("Invalid nationality.", "الجنسية غير صحيحة."),
                        "Invalid Employment Status" => ("Invalid employment status.", "حالة التوظيف غير صحيحة."),
                        "User not found" => ("User not found.", "المستخدم غير موجود."),
                        _ => (messageEn, messageAr)
                    };
                }
            }

            return new ApiErrorResponse(
                messageEn,
                StatusCodes.Status400BadRequest,
                messageAr: messageAr,
                messageEn: messageEn);
        }
    }
}
