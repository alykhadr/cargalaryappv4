namespace CarGalary.Application.Dtos.Brand.Command
{
    public class BulkDeleteBrandsRequest
    {
        public List<int> BrandIds { get; set; } = new();
    }
}
