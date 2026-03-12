namespace CarGalary.Application.Dtos.Request.Query
{
    public class RequestNotificationItemDto
    {
        public int Id { get; set; }
        public string CarName { get; set; } = string.Empty;
        public string? CarImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
