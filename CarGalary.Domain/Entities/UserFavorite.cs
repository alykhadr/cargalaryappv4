namespace CarGalary.Domain.Entities
{
public class UserFavorite
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public int CarId { get; set; }
    public Car Car { get; set; } = null!;

   
    public string? Notes { get; set; }   // "Dream car"
    public int Priority { get; set; }    // ranking favorites

     public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
}