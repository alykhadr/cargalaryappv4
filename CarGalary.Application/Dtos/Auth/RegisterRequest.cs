

using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.Auth
{
    public class RegisterRequest
{
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int BranchId { get; set; }
    public IFormFile? ProfileImage { get; set; }
     // MULTIPLE ROLES
    public List<string>? Roles { get; set;}
}

}
