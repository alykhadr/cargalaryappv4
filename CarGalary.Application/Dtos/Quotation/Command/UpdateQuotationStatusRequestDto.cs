namespace CarGalary.Application.Dtos.Quotation.Command
{
    public class UpdateQuotationStatusRequestDto
    {
        public int CurrentStatus { get; set; }
        public string? Notes { get; set; }
    }
}
