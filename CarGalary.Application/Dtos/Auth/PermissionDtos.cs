namespace CarGalary.Application.Dtos.Auth
{
    public class AddRolePermissionRequest
    {
        public string Page { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
    }

    public class PermissionDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UserListItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsLocked { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BranchId { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
