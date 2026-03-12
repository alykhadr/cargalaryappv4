namespace CarGalary.Application.Dtos.Request.Query
{
    public class RequestHistoryResponseDto
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
