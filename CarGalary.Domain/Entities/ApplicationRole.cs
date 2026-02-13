

using Microsoft.AspNetCore.Identity;

namespace CarGalary.Domain.Entities
{
    public class ApplicationRole :IdentityRole<Guid>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }
     
    }
}