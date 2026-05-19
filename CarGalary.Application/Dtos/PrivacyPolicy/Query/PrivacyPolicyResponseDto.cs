namespace CarGalary.Application.Dtos.PrivacyPolicy.Query
{
    public class PrivacyPolicyResponseDto
    {
        public int Id { get; set; }
        public string? PrivacyPolicyAr { get; set; }
        public string? PrivacyPolicyEn { get; set; }
        public bool IsAvailable { get; set; }
    }
}
