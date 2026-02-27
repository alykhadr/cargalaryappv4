namespace CarGalary.Application.Dtos.Car.Command
{
    public class BulkDeleteCarsRequest
    {
        public List<int> CarIds { get; set; } = new();
    }
}
