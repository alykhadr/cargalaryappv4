using System.Net;
using System.Net.Mail;
using System.IdentityModel.Tokens.Jwt;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.Frontend;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private const string AuthValidationFailedCode = "1101";
        private const string AuthUserNameOrEmailRequiredCode = "1103";
        private const string AuthResetTokenRequiredCode = "1104";
        private const string AuthNewPasswordRequiredCode = "1105";
        private const string AuthNewPasswordMinLengthCode = "1106";
        private const string AuthInvalidResetRequestCode = "1107";
        private const string AuthResetPasswordFailedCode = "1108";
        private const string AuthUnauthorizedCode = "1109";
        private const string AuthEmailAlreadyExistsCode = "1320";
        private const string AuthUserNotFoundCode = "1227";
        private const string DefaultRegisteredUserRole = "User";

        private readonly IIdentityService _identity;
        private readonly IBranchService _branchService;
        private readonly IValidator<RegisterRequest> _registerValidator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IIdentityService identity,
            IBranchService branchService,
            IValidator<RegisterRequest> registerValidator,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _identity = identity;
            _branchService = branchService;
            _registerValidator = registerValidator;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        // ================= REGISTER =================

        [HttpPost("register")]
        [EnableRateLimiting("AuthRegisterPolicy")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var validator = _registerValidator.Validate(request);
                if (!validator.IsValid)
                {
                    var errors = validator.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse(AuthValidationFailedCode, StatusCodes.Status400BadRequest, errors));
                }

                var normalizedEmail = request.Email!.Trim().ToUpperInvariant();
                var normalizedUserName = request.UserName!.Trim();
                var password = request.Password!;

                string emailExist = await _identity.GetUserByEmailAsync(normalizedEmail);
                if (!string.IsNullOrWhiteSpace(emailExist))
                {
                    return BadRequest(new ApiErrorResponse(AuthEmailAlreadyExistsCode));
                }

                var branchId = request.BranchId > 0
                    ? request.BranchId
                    : await ResolveDefaultBranchIdAsync();

                if (branchId <= 0)
                {
                    return BadRequest(new ApiErrorResponse(
                        AuthValidationFailedCode,
                        StatusCodes.Status400BadRequest,
                        new List<string> { "BranchId is required because no default branch is available." }));
                }

                var user = await _identity.CreateUserAsync(
                    normalizedUserName,
                    normalizedEmail,
                    password,
                    request.NameEn?.Trim(),
                    request.NameAr?.Trim(),
                    branchId,
                    null);

                var createdUser = await _userManager.FindByIdAsync(user.Id!);
                if (createdUser != null && !string.IsNullOrWhiteSpace(request.PhoneNumber))
                {
                    createdUser.PhoneNumber = request.PhoneNumber.Trim();
                    await _userManager.UpdateAsync(createdUser);
                }

                if (!await _identity.RoleExistsAsync(DefaultRegisteredUserRole))
                {
                    await _identity.CreateRoleAsync(DefaultRegisteredUserRole);
                }

                await _identity.AssignRoleAsync(user.Id!, DefaultRegisteredUserRole);

                return Ok(new
                {
                    user = createdUser == null ? new FrontendUserDto
                    {
                        Id = user.Id ?? string.Empty,
                        Email = user.Email ?? string.Empty,
                        FullName = user.NameEn,
                        Nickname = user.Username,
                        PhoneNumber = request.PhoneNumber?.Trim(),
                        ProfileComplete = !string.IsNullOrWhiteSpace(user.NameEn)
                    } : ToFrontendUser(createdUser),
                    token = user.Token,
                    needsProfile = false,
                    tokenDetails = BuildTokenDetails(user.Token)
                });
            }
            catch (Exception ex) when (ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new ApiErrorResponse(AuthEmailAlreadyExistsCode));
            }
        }

        [HttpPost("signup")]
        [EnableRateLimiting("AuthRegisterPolicy")]
        public async Task<IActionResult> Signup([FromBody] FrontendSignupRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new ApiErrorResponse(AuthValidationFailedCode));
            }

            var email = request.Email.Trim();
            var existingUserId = await _identity.GetUserByEmailAsync(email.ToUpperInvariant());
            if (!string.IsNullOrWhiteSpace(existingUserId))
            {
                return BadRequest(new ApiErrorResponse(AuthEmailAlreadyExistsCode));
            }

            try
            {
                var user = await _identity.CreateUserAsync(
                    email,
                    email,
                    request.Password,
                    null,
                    null,
                    await ResolveDefaultBranchIdAsync(),
                    null);

                if (!await _identity.RoleExistsAsync(DefaultRegisteredUserRole))
                {
                    await _identity.CreateRoleAsync(DefaultRegisteredUserRole);
                }

                await _identity.AssignRoleAsync(user.Id!, DefaultRegisteredUserRole);

                return Ok(new { userId = user.Id });
            }
            catch (Exception ex) when (ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new ApiErrorResponse(AuthEmailAlreadyExistsCode));
            }
        }

        [HttpPost("fill-profile")]
        public async Task<IActionResult> FillProfile([FromBody] FrontendFillProfileRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserId))
            {
                return BadRequest(new ApiErrorResponse(AuthUserNotFoundCode));
            }

            var user = await _userManager.FindByIdAsync(request.UserId.Trim());
            if (user == null)
            {
                return NotFound(new ApiErrorResponse(AuthUserNotFoundCode, StatusCodes.Status404NotFound));
            }

            ApplyFrontendProfile(user, request);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiErrorResponse(
                    AuthValidationFailedCode,
                    StatusCodes.Status400BadRequest,
                    result.Errors.Select(x => x.Description).ToList()));
            }

            if (!await _identity.RoleExistsAsync(DefaultRegisteredUserRole))
            {
                await _identity.CreateRoleAsync(DefaultRegisteredUserRole);
            }

            if (!await _userManager.IsInRoleAsync(user, DefaultRegisteredUserRole))
            {
                await _identity.AssignRoleAsync(user.Id.ToString(), DefaultRegisteredUserRole);
            }

            var token = await GenerateJwtForUserAsync(user);
            return Ok(new
            {
                token,
                user = ToFrontendUser(user),
                tokenDetails = BuildTokenDetails(token)
            });
        }

        
        // ================= LOGIN =================

        [HttpPost("login")]
        [EnableRateLimiting("AuthLoginPolicy")]
        public async Task<IActionResult> Login(LoginRequest request,
        [FromServices] IValidator<LoginRequest> _validator)
        {
            try
            {
                var userNameOrEmail = !string.IsNullOrWhiteSpace(request.UserName)
                    ? request.UserName.Trim()
                    : request.Email?.Trim();

                var normalizedRequest = new LoginRequest
                {
                    UserName = userNameOrEmail,
                    Email = request.Email,
                    Password = request.Password,
                    RememberMe = request.RememberMe
                };

                var validator = _validator.Validate(normalizedRequest);
                if (!validator.IsValid)
                {
                    var errors = validator.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse(AuthValidationFailedCode, StatusCodes.Status400BadRequest, errors));
                }
                var user = await _identity.LoginAsync(
                    normalizedRequest.UserName!.Trim(),
                    normalizedRequest.Password,
                    normalizedRequest.RememberMe);

                ApplicationUser? applicationUser = null;
                if (!string.IsNullOrWhiteSpace(user.Id))
                {
                    applicationUser = await _userManager.FindByIdAsync(user.Id);
                }

                var frontendUser = applicationUser == null
                    ? new FrontendUserDto
                    {
                        Id = user.Id ?? string.Empty,
                        Email = user.Email ?? string.Empty,
                        FullName = user.NameEn,
                        Nickname = user.Username,
                        ProfileComplete = !string.IsNullOrWhiteSpace(user.NameEn)
                    }
                    : ToFrontendUser(applicationUser);

                return Ok(new
                {
                    user = frontendUser,
                    token = user.Token,
                    needsProfile = !frontendUser.ProfileComplete,
                    tokenDetails = BuildTokenDetails(user.Token)
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new ApiErrorResponse(
                    AuthUnauthorizedCode,
                    StatusCodes.Status401Unauthorized,
                    errorCode: AuthUnauthorizedCode));
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { success = true });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized(new ApiErrorResponse(
                    AuthUnauthorizedCode,
                    StatusCodes.Status401Unauthorized,
                    errorCode: AuthUnauthorizedCode));
            }

            return Ok(new { user = ToFrontendUser(user) });
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] FrontendUpdateProfileRequest request)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized(new ApiErrorResponse(
                    AuthUnauthorizedCode,
                    StatusCodes.Status401Unauthorized,
                    errorCode: AuthUnauthorizedCode));
            }

            ApplyFrontendProfile(user, request);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiErrorResponse(
                    AuthValidationFailedCode,
                    StatusCodes.Status400BadRequest,
                    result.Errors.Select(x => x.Description).ToList()));
            }

            return Ok(new { user = ToFrontendUser(user) });
        }

        // ================= FORGOT/RESET PASSWORD =================

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserNameOrEmail))
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

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserNameOrEmail))
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

            return Ok(new
            {
                message = IsArabicRequest()
                    ? "تمت إعادة تعيين كلمة المرور بنجاح"
                    : "Password reset successfully"
            });
        }

        // ================= DELETE USER =================

       
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
            var isArabic = IsArabicRequest();
            var subject = isArabic ? "إعادة تعيين كلمة المرور" : "Reset your password";
            var body = isArabic
                ? $"<p>لقد طلبت إعادة تعيين كلمة المرور.</p><p><a href=\"{resetLink}\">اضغط هنا لإعادة تعيين كلمة المرور</a></p><p>إذا لم تطلب ذلك، يمكنك تجاهل هذه الرسالة.</p>"
                : $"<p>You requested a password reset.</p><p><a href=\"{resetLink}\">Click here to reset your password</a></p><p>If you did not request this, ignore this email.</p>";

            using var message = new MailMessage(from, recipientEmail)
            {
                Subject = subject,
                Body = body,
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

        private bool IsArabicRequest()
        {
            var acceptLanguage = HttpContext.Request.Headers.AcceptLanguage.ToString();
            if (string.IsNullOrWhiteSpace(acceptLanguage))
            {
                return false;
            }

            return acceptLanguage
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Any(lang => lang.StartsWith("ar", StringComparison.OrdinalIgnoreCase));
        }

        private async Task<ApplicationUser?> GetCurrentUserAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrWhiteSpace(userId)
                ? null
                : await _userManager.FindByIdAsync(userId);
        }

        private void ApplyFrontendProfile(ApplicationUser user, FrontendFillProfileRequest request)
        {
            ApplyFrontendProfile(user, new FrontendUpdateProfileRequest
            {
                FullName = request.FullName,
                Nickname = request.Nickname,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth,
                Country = request.Country,
                AvatarUrl = request.AvatarUrl
            });
        }

        private static void ApplyFrontendProfile(ApplicationUser user, FrontendUpdateProfileRequest? request)
        {
            if (request == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(request.FullName))
            {
                user.FullNameEn = request.FullName.Trim();
                user.FullNameAr = request.FullName.Trim();
            }

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                user.PhoneNumber = request.PhoneNumber.Trim();
            }

            if (!string.IsNullOrWhiteSpace(request.Country))
            {
                user.Address = request.Country.Trim();
            }

            if (!string.IsNullOrWhiteSpace(request.AvatarUrl))
            {
                user.ProfileImageUrl = request.AvatarUrl.Trim();
            }
        }

        private static FrontendUserDto ToFrontendUser(ApplicationUser user)
        {
            var email = user.Email ?? string.Empty;
            var nickname = string.Equals(user.UserName, email, StringComparison.OrdinalIgnoreCase)
                ? null
                : user.UserName;

            return new FrontendUserDto
            {
                Id = user.Id.ToString(),
                Email = email,
                FullName = user.FullNameEn ?? user.FullNameAr,
                Nickname = nickname,
                PhoneNumber = user.PhoneNumber,
                Country = user.Address,
                AvatarUrl = user.ProfileImageUrl,
                ProfileComplete = !string.IsNullOrWhiteSpace(user.FullNameEn) ||
                    !string.IsNullOrWhiteSpace(user.FullNameAr)
            };
        }

        private async Task<int> ResolveDefaultBranchIdAsync()
        {
            var branches = await _branchService.GetAllAsync();
            return branches
                .Where(x => x.IsAvailable)
                .OrderBy(x => x.Id)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        private async Task<string> GenerateJwtForUserAsync(ApplicationUser user, bool rememberMe = false)
        {
            var utcNow = DateTime.UtcNow;
            user.LastLoginAt = utcNow;
            user.LastActivityAt = utcNow;
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    continue;
                }

                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claims.Where(c => c.Type == "permission" && !string.IsNullOrWhiteSpace(c.Value)))
                {
                    permissions.Add(claim.Value.Trim());
                }
            }

            var jwtSection = _configuration.GetSection("Jwt");
            var key = jwtSection["Key"];
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];
            var expiryMinutes = int.TryParse(jwtSection["ExpiryMinutes"], out var configuredExpiry)
                ? configuredExpiry
                : 1440;

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("JWT key is not configured.");
            }

            var tokenClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new("branch_id", user.BranchId.ToString(CultureInfo.InvariantCulture))
            };

            if (!string.IsNullOrWhiteSpace(user.UserName))
            {
                tokenClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            }

            tokenClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            tokenClaims.AddRange(permissions.Select(permission => new Claim("permission", permission)));

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: tokenClaims,
                expires: rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static object? BuildTokenDetails(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
            {
                return null;
            }

            var jwt = handler.ReadJwtToken(token);
            var expiresAtUtc = jwt.ValidTo;
            var issuedAtUtc = jwt.IssuedAt;
            var notBeforeUtc = jwt.ValidFrom;
            var expiresInSeconds = Math.Max(0, (long)(expiresAtUtc - DateTime.UtcNow).TotalSeconds);

            return new
            {
                tokenType = "Bearer",
                expiresAtUtc,
                issuedAtUtc,
                notBeforeUtc,
                expiresInSeconds,
                issuer = jwt.Issuer,
                audiences = jwt.Audiences.ToList(),
                claims = jwt.Claims.Select(c => new { c.Type, c.Value }).ToList()
            };
        }
    }
}
