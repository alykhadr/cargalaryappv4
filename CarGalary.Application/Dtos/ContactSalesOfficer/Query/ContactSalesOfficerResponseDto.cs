namespace CarGalary.Application.Dtos.ContactSalesOfficer.Query
{
    public class ContactSalesOfficerResponseDto
    {
        public int Id { get; set; }
        public string? ContactValue { get; set; }
        public bool IsMobileNo { get; set; }
        public bool IsAvailable { get; set; }
    }
}
