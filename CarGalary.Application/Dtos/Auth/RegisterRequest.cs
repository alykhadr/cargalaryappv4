

namespace CarGalary.Application.Dtos.Auth
{
    public class RegisterRequest
{
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
     // MULTIPLE ROLES
    public List<string>? Roles { get; set;}
}

}
