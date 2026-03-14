using System.Net;
using System.Net.Mail;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarGalary.Domain.Entities;

namespace CarGalary.Admin.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private const string AuthValidationFailedCode = "1101";
        private const string AuthInternalServerErrorCode = "1102";
        private const string AuthUserNameOrEmailRequiredCode = "1103";
        private const string AuthResetTokenRequiredCode = "1104";
        private const string AuthNewPasswordRequiredCode = "1105";
        private const string AuthNewPasswordMinLengthCode = "1106";
        private const string AuthInvalidResetRequestCode = "1107";
        private const string AuthResetPasswordFailedCode = "1108";
        private const string AuthUnauthorizedCode = "1109";
        private const string AuthUserNotFoundCode = "1227";

        private readonly IIdentityService _identity;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IIdentityService identity,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _identity = identity;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request,
                                [FromServices] IValidator<LoginRequest> validator)
        {
            try
            {
                var validation = validator.Validate(request);
                if (!validation.IsValid)
                {
                    var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse(AuthValidationFailedCode, StatusCodes.Status400BadRequest, errors));
                }

                var user = await _identity.LoginAsync(
                    request.UserName.Trim(),
                    request.Password,
                    request.RememberMe);

                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiErrorResponse(
                    ex.Message,
                    StatusCodes.Status401Unauthorized,
                    errorCode: AuthUnauthorizedCode));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(AuthInternalServerErrorCode, StatusCodes.Status500InternalServerError));
            }
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserNameOrEmail))
            {
                return BadRequest(new ApiErrorResponse(AuthUserNameOrEmailRequiredCode));
            }

            var identifier = request.UserNameOrEmail.Trim();
            var user = await FindByUserNameOrEmailOrNullAsync(identifier);

            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest(new ApiErrorResponse(AuthUserNotFoundCode));
            }

            if (user != null)
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

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserNameOrEmail))
            {
                return BadRequest(new ApiErrorResponse(AuthUserNameOrEmailRequiredCode));
            }

            if (string.IsNullOrWhiteSpace(request.Token))
            {
                return BadRequest(new ApiErrorResponse(AuthResetTokenRequiredCode));
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest(new ApiErrorResponse(AuthNewPasswordRequiredCode));
            }

            if (request.NewPassword.Length < 6)
            {
                return BadRequest(new ApiErrorResponse(AuthNewPasswordMinLengthCode));
            }

            var user = await FindByUserNameOrEmailOrNullAsync(request.UserNameOrEmail.Trim());
            if (user == null)
            {
                return BadRequest(new ApiErrorResponse(AuthInvalidResetRequestCode));
            }

            var decodedToken = request.Token.Trim().Replace(" ", "+");
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new ApiErrorResponse(AuthResetPasswordFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            return Ok(new { message = "Password reset successfully" });
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
