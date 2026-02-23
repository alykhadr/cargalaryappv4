namespace CarGalary.Application.Dtos.Auth
{
    public class BulkDeleteUsersRequest
    {
        public List<string> UserIds { get; set; } = new();
    }
}
