namespace CarGalary.Application.Dtos.FAQ.Command
{
    public class BulkDeleteFAQRequest
    {
        public List<int> FaqIds { get; set; } = new();
    }
}
