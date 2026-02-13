

namespace CarGalary.Domain.Entities
{
    public class UserProfile:BaseEntity
    {


        // One-to-one relationship with User
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ProfileImageUrl { get; set; }

      
        public string? CreatedBy { get; set; }
        
    }
}