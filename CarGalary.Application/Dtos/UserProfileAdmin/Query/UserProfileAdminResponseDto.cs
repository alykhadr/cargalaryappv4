namespace CarGalary.Application.Dtos.UserProfileAdmin.Query
{
    public class UserProfileAdminResponseDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsAvailable { get; set; }
    }
}
