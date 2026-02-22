namespace CarGalary.Application.Dtos.Auth
{
    public class ForgotPasswordRequest
    {
        public string UserNameOrEmail { get; set; } = string.Empty;
    }

    public class ForgotPasswordResponse
    {
        public string Message { get; set; } = "If the account exists, a password reset token has been generated.";
        public string? ResetToken { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string UserNameOrEmail { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
