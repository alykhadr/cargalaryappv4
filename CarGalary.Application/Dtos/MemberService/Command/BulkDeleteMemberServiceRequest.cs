namespace CarGalary.Application.Dtos.MemberService.Command
{
    public class BulkDeleteMemberServiceRequest
    {
        public List<int> MemberServiceIds { get; set; } = new();
    }
}
