namespace CarGalary.Application.Dtos.Auth
{
    public class UpdateAdminUserRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public class ChangeUserPasswordByAdminRequest
    {
        public string NewPassword { get; set; } = string.Empty;
    }
}
