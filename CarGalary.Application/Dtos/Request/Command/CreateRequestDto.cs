namespace CarGalary.Application.Dtos.Request.Command
{
    public class CreateRequestDto
    {
        public Guid? UserId { get; set; }
        public int VehicleOwnerType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public int CarId { get; set; }
        public int ColorId { get; set; }
        public int PaymentMethod { get; set; }
        public int RegionId { get; set; }
        public int CityId { get; set; }
        public string? Notes { get; set; }
    }
}
