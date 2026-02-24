namespace CarGalary.Application.Dtos.Services.Query
{
    public class BulkDeleteServicesResponseDto
    {
        public int DeletedCount { get; set; }
        public List<int> FailedIds { get; set; } = new();
    }
}
