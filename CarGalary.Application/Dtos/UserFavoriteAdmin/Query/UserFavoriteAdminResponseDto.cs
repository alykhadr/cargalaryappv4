namespace CarGalary.Application.Dtos.UserFavoriteAdmin.Query
{
    public class UserFavoriteAdminResponseDto
    {
        public Guid UserId { get; set; }
        public int CarId { get; set; }
        public string? Notes { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
