namespace CarGalary.Application.Dtos.Package.Command
{
    public class BulkDeletePackagesRequest
    {
        public List<int> PackageIds { get; set; } = new();
    }
}
