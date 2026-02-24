namespace CarGalary.Application.Dtos.CarModel.Query
{
    public class BulkDeleteCarModelResponseDto
    {
        public int DeletedCount { get; set; }
        public List<int> FailedIds { get; set; } = new();
    }
}
