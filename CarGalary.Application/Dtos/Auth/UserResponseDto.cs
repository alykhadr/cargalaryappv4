namespace CarGalary.Application.Dtos.Auth
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string NameEn { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public string BranchNameEn { get; set; } = string.Empty;
        public string BranchNameAr { get; set; } = string.Empty;
        public string? Token { get; set; }
        public string? Email { get; set; }
    }
}
