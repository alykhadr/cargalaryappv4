

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

   
    }
}