namespace CarGalary.Application.Dtos.Request.Query
{
    public class RequestNotificationsResponseDto
    {
        public int Count { get; set; }
        public List<RequestNotificationItemDto> Items { get; set; } = new();
    }
}
