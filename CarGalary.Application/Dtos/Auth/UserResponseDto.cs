using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarGalary.Application.Dtos.Auth
{
    public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}
}