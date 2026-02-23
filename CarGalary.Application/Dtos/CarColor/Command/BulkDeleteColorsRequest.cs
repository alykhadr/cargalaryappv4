namespace CarGalary.Application.Dtos.CarColor.Command
{
    public class BulkDeleteColorsRequest
    {
        public List<int> ColorIds { get; set; } = new();
    }
}
