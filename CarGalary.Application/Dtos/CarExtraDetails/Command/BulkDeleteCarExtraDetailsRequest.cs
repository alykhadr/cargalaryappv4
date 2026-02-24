namespace CarGalary.Application.Dtos.CarExtraDetails.Command
{
    public class BulkDeleteCarExtraDetailsRequest
    {
        public List<int> Ids { get; set; } = new();
    }
}
