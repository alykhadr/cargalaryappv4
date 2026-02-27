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
        public int EmployeeId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? MobileNo { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsLocked { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BranchId { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string EmployeeNo { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string EmploymentStatus { get; set; } = string.Empty;
        public string? WorkEmail { get; set; }
        public string? WorkPhone { get; set; }
        public string? Extension { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
    }
}
