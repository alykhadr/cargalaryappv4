namespace CarGalary.Application.Dtos.User.Query
{
    public class UserByBranchResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? BranchId { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
