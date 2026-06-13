namespace CarGalary.Application.Dtos.PrivacyPolicy.Command
{
    public class BulkDeletePrivacyPolicyRequest
    {
        public List<int> PrivacyPolicyIds { get; set; } = new();
    }
}
