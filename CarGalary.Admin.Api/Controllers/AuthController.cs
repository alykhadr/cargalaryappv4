
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identity;
        private readonly IValidator<RegisterRequest> _registerValidator;

        public AuthController(IIdentityService identity,
        IValidator<RegisterRequest> registerValidator)
        {
            _identity = identity;
            this._registerValidator = registerValidator;
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
                string emailExist=await _identity.GetUserByEmailAsync(request.Email.ToUpper().Trim());
                if (!string.IsNullOrWhiteSpace(emailExist))
                {
                    return BadRequest(new ApiErrorResponse($"email : {request.Email} already exist"));
                }
                // Force default role for public registration
                var roles = (request.Roles != null && request.Roles.Any()) ? request.Roles
                    : new List<string> { "User" };
                    
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiErrorResponse("Internal server error", StatusCodes.Status500InternalServerError));
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
                    request.Password);

                return Ok(user);

                
            }
            catch (Exception ex)
            {





                
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiErrorResponse("Internal server error", StatusCodes.Status500InternalServerError));
            }
        }

        // ================= DELETE USER =================

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
    }

}
