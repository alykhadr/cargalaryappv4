namespace CarGalary.Application.Dtos.Car.Query
{
    public class BulkDeleteCarsResponseDto
    {
        public int DeletedCount { get; set; }
        public List<int> FailedIds { get; set; } = new();
    }
}
