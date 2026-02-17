namespace CarGalary.Application.Dtos.UserProfileAdmin.Command
{
    public class CreateUserProfileAdminRequestDto
    {
        public Guid UserId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? CreatedBy { get; set; }
    }
}
