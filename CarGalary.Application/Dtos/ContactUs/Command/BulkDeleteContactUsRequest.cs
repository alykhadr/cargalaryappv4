namespace CarGalary.Application.Dtos.ContactUs.Command
{
    public class BulkDeleteContactUsRequest
    {
        public List<int> ContactIds { get; set; } = new();
    }
}
