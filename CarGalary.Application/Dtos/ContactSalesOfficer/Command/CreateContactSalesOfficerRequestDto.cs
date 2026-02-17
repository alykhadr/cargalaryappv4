namespace CarGalary.Application.Dtos.ContactSalesOfficer.Command
{
    public class CreateContactSalesOfficerRequestDto
    {
        public string? ContactValue { get; set; }
        public bool IsMobileNo { get; set; } = true;
    }
}
