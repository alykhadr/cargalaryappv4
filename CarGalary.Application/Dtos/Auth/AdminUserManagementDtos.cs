using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.Auth
{
    public class UpdateAdminUserRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int BranchId { get; set; }
        public IFormFile? ProfileImage { get; set; }

        // Employee details
        public string? EmployeeNo { get; set; }
        public string? NationalId { get; set; }
        public string? JobTitle { get; set; }
        public int DepartmentId { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string? EmploymentStatus { get; set; }
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

    public class ChangeUserPasswordByAdminRequest
    {
        public string NewPassword { get; set; } = string.Empty;
    }
}
