namespace CarGalary.Application.Dtos.Quotation.Query
{
    public class QuotationNotificationItemDto
    {
        public int Id { get; set; }
        public string CarName { get; set; } = string.Empty;
        public string? CarImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
