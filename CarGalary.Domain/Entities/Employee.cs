namespace CarGalary.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;

        public int BranchId { get; set; }
        public Branchs Branch { get; set; } = default!;

        public string EmployeeNo { get; set; } = "";
        public string NationalId { get; set; } = "";

        public string JobTitle { get; set; } = "";
        public string Department { get; set; } = "";
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string EmploymentStatus { get; set; } = "Active";

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
