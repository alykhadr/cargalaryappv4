namespace CarGalary.Application.Dtos.Quotation.Query
{
    public class QuotationHistoryResponseDto
    {
        public int Id { get; set; }
        public int QuotationId { get; set; }
        public int Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
