namespace CarGalary.Application.Dtos.Request.Query
{
    public class RequestResponseDto
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public int VehicleOwnerType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public int CarId { get; set; }
        public int ColorId { get; set; }
        public string? ColorNameAr { get; set; }
        public string? ColorNameEn { get; set; }
        public int PaymentMethod { get; set; }
        public int RegionId { get; set; }
        public int CityId { get; set; }
        public int CurrentStatus { get; set; }
        public string? CurrentStatusNameAr { get; set; }
        public string? CurrentStatusNameEn { get; set; }
        public DateTime? CurrentStatusDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAvailable { get; set; }
    }
}
