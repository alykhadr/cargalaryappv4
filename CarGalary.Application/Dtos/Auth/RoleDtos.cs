namespace CarGalary.Application.Dtos.Auth
{
    public class RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateRoleRequest
    {
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class UpdateRoleRequest
    {
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
