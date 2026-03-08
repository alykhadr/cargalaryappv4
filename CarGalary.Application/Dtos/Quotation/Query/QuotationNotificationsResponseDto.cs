namespace CarGalary.Application.Dtos.Quotation.Query
{
    public class QuotationNotificationsResponseDto
    {
        public int Count { get; set; }
        public List<QuotationNotificationItemDto> Items { get; set; } = new();
    }
}
