using System.Net;
using System.Net.Mail;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Admin.Api.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identity;
        private readonly IValidator<RegisterRequest> _registerValidator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IIdentityService identity,
            IValidator<RegisterRequest> registerValidator,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _identity = identity;
            _registerValidator = registerValidator;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        // ================= REGISTER =================

        // [Authorize(Roles = "Admin")]
        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterByAdmin(RegisterRequest request)
        {
            try
            {
                var validator = _registerValidator.Validate(request);
                if (!validator.IsValid)
                {
                    var errors = validator.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
                }

                string emailExist = await _identity.GetUserByEmailAsync(request.Email.ToUpper().Trim());
                if (!string.IsNullOrWhiteSpace(emailExist))
                {
                    return BadRequest(new ApiErrorResponse($"email : {request.Email} already exist"));
                }

                var user = await _identity.CreateUserAsync(
                    request.UserName.Trim(),
                    request.Email.ToUpper().Trim(),
                    request.Password,
                    request.FirstName?.Trim(),
                    request.LastName?.Trim());

                var userRoles = request.Roles ?? new List<string>();

                foreach (var role in userRoles)
                {
                    await _identity.AssignRoleAsync(user.Id!, role);
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new ApiErrorResponse(ex.Message));
                }

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse("Internal server error", StatusCodes.Status500InternalServerError));
            }
        }

        // ================= LOGIN =================

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request,
                                [FromServices] IValidator<LoginRequest> _validator)
        {
            try
            {
                var validator = _validator.Validate(request);
                if (!validator.IsValid)
                {
                    var errors = validator.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
                }

                var user = await _identity.LoginAsync(
                    request.UserName.Trim(),
                    request.Password,
                    request.RememberMe);

                return Ok(user);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse("Internal server error", StatusCodes.Status500InternalServerError));
            }
        }

        // ================= FORGOT/RESET PASSWORD =================

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserNameOrEmail))
            {
                return BadRequest(new ApiErrorResponse("User name or email is required"));
            }

            var identifier = request.UserNameOrEmail.Trim();
            var user = await FindByUserNameOrEmailOrNullAsync(identifier);

            if (user != null && !string.IsNullOrWhiteSpace(user.Email))
            {
                try
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetLink = BuildPasswordResetLink(user.UserName ?? user.Email, token);
                    await TrySendPasswordResetEmailAsync(user.Email, resetLink);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process forgot password email for account '{Identifier}'", identifier);
                }
            }

            return Ok(new ForgotPasswordResponse());
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserNameOrEmail))
            {
                return BadRequest(new ApiErrorResponse("User name or email is required"));
            }

            if (string.IsNullOrWhiteSpace(request.Token))
            {
                return BadRequest(new ApiErrorResponse("Reset token is required"));
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest(new ApiErrorResponse("New password is required"));
            }

            if (request.NewPassword.Length < 6)
            {
                return BadRequest(new ApiErrorResponse("New password must be at least 6 characters"));
            }

            var user = await FindByUserNameOrEmailOrNullAsync(request.UserNameOrEmail.Trim());
            if (user == null)
            {
                return BadRequest(new ApiErrorResponse("Invalid reset request"));
            }

            var decodedToken = request.Token.Trim().Replace(" ", "+");
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new ApiErrorResponse("Password reset failed", StatusCodes.Status400BadRequest, errors));
            }

            return Ok(new { message = "Password reset successfully" });
        }

        // ================= USER MANAGEMENT =================

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _identity.GetUsersAsync();
            return Ok(users);
        }

        [HttpPut("users/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateAdminUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new ApiErrorResponse("Username and email are required"));
            }

            await _identity.UpdateUserDetailsAsync(
                userId,
                request.UserName,
                request.Email,
                request.FirstName,
                request.LastName
            );

            return Ok();
        }

        [HttpPost("users/{userId}/change-password")]
        public async Task<IActionResult> ChangeUserPassword(string userId, [FromBody] ChangeUserPasswordByAdminRequest request)
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

        //[Authorize(Roles = "Admin")]
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _identity.DeleteUserAsync(userId);

            if (!result)
                return NotFound(new ApiErrorResponse("User not found", StatusCodes.Status404NotFound));

            return NoContent();
        }

        // ================= LOCK USER =================

        //  [Authorize(Roles = "Admin")]
        [HttpPost("users/{userId}/lock")]
        public async Task<IActionResult> LockUser(string userId)
        {
            await _identity.LockUserAsync(userId);
            return Ok("User locked");
        }

        // ================= UNLOCK USER =================

        // [Authorize(Roles = "Admin")]
        [HttpPost("users/{userId}/unlock")]
        public async Task<IActionResult> UnlockUser(string userId)
        {
            await _identity.UnlockUserAsync(userId);
            return Ok("User unlocked");
        }

        // ================= GET USER ROLES =================

        //[Authorize]
        [HttpGet("users/{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var roles = await _identity.GetUserRolesAsync(userId);
            return Ok(roles);
        }

        [HttpGet("users/{userId}/permissions")]
        public async Task<IActionResult> GetUserPermissions(string userId)
        {
            var permissions = await _identity.GetUserPermissionsAsync(userId);
            return Ok(permissions);
        }

        // ================= ASSIGN ROLE =================

        // [Authorize(Roles = "Admin")]
        [HttpPost("users/{userId}/roles/{role}")]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            await _identity.AssignRoleAsync(userId, role);
            return Ok("Role assigned");
        }

        // ================= REMOVE ROLE =================

        //[Authorize(Roles = "Admin")]
        [HttpDelete("users/{userId}/roles/{role}")]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            await _identity.RemoveRoleAsync(userId, role);
            return Ok("Role removed");
        }

        private async Task<ApplicationUser?> FindByUserNameOrEmailOrNullAsync(string userNameOrEmail)
        {
            var normalized = userNameOrEmail.Trim().ToUpperInvariant();

            var byUserName = await _userManager.Users
                .FirstOrDefaultAsync(u => u.NormalizedUserName == normalized);
            if (byUserName != null)
            {
                return byUserName;
            }

            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalized);
        }

        private string BuildPasswordResetLink(string userNameOrEmail, string token)
        {
            var baseUrl = _configuration["ClientApp:BaseUrl"]?.TrimEnd('/') ?? "http://localhost:4200";
            var encodedToken = Uri.EscapeDataString(token);
            var encodedUser = Uri.EscapeDataString(userNameOrEmail);
            return $"{baseUrl}/auth/pass-reset/basic?token={encodedToken}&user={encodedUser}";
        }

        private async Task TrySendPasswordResetEmailAsync(string recipientEmail, string resetLink)
        {
            var host = _configuration["Email:SmtpHost"];
            var from = _configuration["Email:From"];

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(from))
            {
                _logger.LogWarning("Email settings are not configured. Skipping password reset email for {Email}", recipientEmail);
                return;
            }

            var port = int.TryParse(_configuration["Email:SmtpPort"], out var smtpPort) ? smtpPort : 587;
            var enableSsl = !bool.TryParse(_configuration["Email:EnableSsl"], out var parsedSsl) || parsedSsl;
            var username = _configuration["Email:SmtpUser"];
            var password = _configuration["Email:SmtpPassword"];

            using var message = new MailMessage(from, recipientEmail)
            {
                Subject = "Reset your password",
                Body = $"<p>You requested a password reset.</p><p><a href=\"{resetLink}\">Click here to reset your password</a></p><p>If you did not request this, ignore this email.</p>",
                IsBodyHtml = true
            };

            using var smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            if (!string.IsNullOrWhiteSpace(username))
            {
                smtpClient.Credentials = new NetworkCredential(username, password ?? string.Empty);
            }

            await smtpClient.SendMailAsync(message);
        }
    }
}
