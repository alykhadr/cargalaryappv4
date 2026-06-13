namespace CarGalary.Application.Dtos.PrivacyPolicy.Command
{
    public class UpdatePrivacyPolicyRequestDto
    {
        public string? PrivacyPolicyAr { get; set; }
        public string? PrivacyPolicyEn { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
