

namespace CarGalary.Api.Dtos
{
    public class UserProfileResponseDto
    {
        public int UserId { get; set; }
        public string? Username { get; set; } // optional, can map from User
        public string? Email { get; set; }    // optional
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ProfileImageUrl { get; set; }
        
    }
}