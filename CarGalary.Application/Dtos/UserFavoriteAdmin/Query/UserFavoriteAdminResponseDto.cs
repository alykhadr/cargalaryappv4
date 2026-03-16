namespace CarGalary.Application.Dtos.UserFavoriteAdmin.Query
{
    public class UserFavoriteAdminResponseDto
    {
        public Guid UserId { get; set; }
        public int CarId { get; set; }
        public string? CarNameAr { get; set; }
        public string? CarNameEn { get; set; }
        public int? ModelId { get; set; }
        public string? ModelNameAr { get; set; }
        public string? ModelNameEn { get; set; }
        public int? BrandId { get; set; }
        public string? BrandNameAr { get; set; }
        public string? BrandNameEn { get; set; }
        public string? Notes { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
