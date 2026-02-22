namespace CarGalary.Application.Dtos.Auth
{
    public class ForgotPasswordRequest
    {
        public string UserNameOrEmail { get; set; } = string.Empty;
    }

    public class ForgotPasswordResponse
    {
        public string Message { get; set; } = "If the account exists, a password reset link has been sent.";
    }

    public class ResetPasswordRequest
    {
        public string UserNameOrEmail { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
