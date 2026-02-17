namespace CarGalary.Application.Dtos.UserFavoriteAdmin.Command
{
    public class CreateUserFavoriteAdminRequestDto
    {
        public Guid UserId { get; set; }
        public int CarId { get; set; }
        public string? Notes { get; set; }
        public int Priority { get; set; }
    }
}
