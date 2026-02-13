
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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
                    // Return all errors as an array of strings
                    var errors = validator.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
                string emailExist=await _identity.GetUserByEmailAsync(request.Email.ToUpper().Trim());
                if (!string.IsNullOrWhiteSpace(emailExist))
                {
                    return BadRequest(new {error= $"email : {request.Email} already exist"});
                }
                // Force default role for public registration
                var roles = (request.Roles != null && request.Roles.Any()) ? request.Roles
                    : new List<string> { "User" };
                    
                var userId = await _identity.CreateUserAsync(
                    request.UserName.Trim(),
                   request.Email.ToUpper().Trim(),
                    request.Password);

                foreach (var role in request.Roles)
                {
                    await _identity.AssignRoleAsync(userId, role);
                }

                var token = await _identity.LoginAsync(request.UserName, request.Password);
                return Ok(new { UserId = userId, Token = token, Roles = request.Roles });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
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
                    // Return all errors as an array of strings
                    var errors = validator.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
                var token = await _identity.LoginAsync(
                    request.UserName.Trim(),
                    request.Password);

                return Ok(new
                {
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        // ================= DELETE USER =================

        //[Authorize(Roles = "Admin")]
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _identity.DeleteUserAsync(userId);

            if (!result)
                return NotFound("User not found");

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