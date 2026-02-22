using Microsoft.AspNetCore.Identity;

namespace CarGalary.Domain.Entities
{

    public class ApplicationUser : IdentityUser<Guid>
    {

        public string? FullNameAr { get; set; }
        public string? FullNameEn { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsAvailable { get; set; } = true;
        public string? CreatedBy { get; set; }
       public string? Address { get; set; }
        public string? ProfileImageUrl { get; set; }

        public ICollection<UserFavorite> Favorites { get; set; }
           = new List<UserFavorite>();
    }
}
