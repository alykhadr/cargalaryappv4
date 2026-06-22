namespace CarGalary.Application.Dtos.Auth
{
   public class LoginRequest
    {
        public string? Email { get; set; }
        public string? UserName { get; set; } // may be email or user name
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
