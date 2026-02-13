

using CarGalary.Application.Dtos;
using CarGalary.Application.Dtos.UserProfile;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [Route("api/profile")]
    [Authorize]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserProfileService _profileService;
        private readonly IIdentityService _identityService;

        public ProfileController(IUserProfileService profileService, IIdentityService identityService)
        {
            _profileService = profileService;
            this._identityService = identityService;
        }

 [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
    {
        // Use current user id if you want
        

        await _identityService.ChangePasswordAsync(command.UserId, command.CurrentPassword, command.NewPassword);
        return Ok("Password changed successfully");
    }

    [HttpPost("update-email")]
    public async Task<IActionResult> UpdateEmail([FromBody] string newEmail)
    {
        string userId="";
        await _identityService.UpdateEmailAsync(userId!, newEmail);
        return Ok("Email updated successfully");
    }

    [HttpPost("update-username")]
    public async Task<IActionResult> UpdateUsername([FromBody] string newUsername)
    {
        var userId = "";
        await _identityService.UpdateUsernameAsync(userId!, newUsername);
        return Ok("Username updated successfully");
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = "";
        await _identityService.DeleteUserAsync(userId!);
        return NoContent();
    }
        // // GET: api/UserProfiles/me
        // [HttpGet("me")]
        // public async Task<IActionResult> GetMyProfile()
        // {
        //     // Example: get UserId from JWT claims
        //     var userId = int.Parse(User.FindFirst("id")!.Value);
        //     var profile = await _profileService.GetProfileByUserIdAsync(userId);
        //     if (profile == null) return NotFound(new { message = "Profile not found" });

        //     return Ok(profile);
        // }

        // // POST: api/UserProfiles
        // [HttpPost]
        // public async Task<IActionResult> CreateProfile([FromBody] CreateUserProfileRequestDto dto)
        // {
        //     var userId = int.Parse(User.FindFirst("id")!.Value);
        //     var profile = await _profileService.CreateProfileAsync(userId, dto, User.Identity?.Name);
        //     return Ok(profile);
        // }

        // // PUT: api/UserProfiles
        // [HttpPut]
        // public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileRequestDto dto)
        // {
        //     var userId = int.Parse(User.FindFirst("id")!.Value);
        //     var profile = await _profileService.UpdateProfileAsync(userId, dto);
        //     if (profile == null) return NotFound(new { message = "Profile not found" });

        //     return Ok(profile);
        // }

        // // DELETE: api/UserProfiles
        // [HttpDelete]
        // public async Task<IActionResult> DeleteProfile()
        // {
        //     var userId = int.Parse(User.FindFirst("id")!.Value);
        //     var deleted = await _profileService.DeleteProfileAsync(userId);
        //     if (!deleted) return NotFound(new { message = "Profile not found" });
        //     return NoContent();
        // }
    }
}