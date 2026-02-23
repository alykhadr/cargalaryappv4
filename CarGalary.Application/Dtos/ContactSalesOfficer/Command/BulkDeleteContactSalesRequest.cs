namespace CarGalary.Application.Dtos.ContactSalesOfficer.Command
{
    public class BulkDeleteContactSalesRequest
    {
        public List<int> ContactIds { get; set; } = new();
    }
}
