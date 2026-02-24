namespace CarGalary.Application.Dtos.CarModel.Command
{
    public class BulkDeleteCarModelRequestDto
    {
        public List<int> ModelIds { get; set; } = new();
    }
}
