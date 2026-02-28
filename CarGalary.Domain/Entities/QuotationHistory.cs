namespace CarGalary.Domain.Entities
{
    public class QuotationHistory : BaseEntity
    {
        public int QuotationId { get; set; }
        public Quotation Quotation { get; set; } = default!;

        public int Status { get; set; }
        public LookupDetails? StatusLookup { get; set; }

        public DateTime StatusDate { get; set; }
        public string? Notes { get; set; }
    }
}
