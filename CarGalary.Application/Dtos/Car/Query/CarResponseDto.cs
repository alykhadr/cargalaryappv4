namespace CarGalary.Application.Dtos.Car.Query
{
    public class CarResponseDto
    {
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int ModelId { get; set; }
        public int TypeId { get; set; }
        public int BranchId { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public decimal? Vat { get; set; }
        public int? ConditionId { get; set; }
        public int? SeatingCapacity { get; set; }
        public string? WeelSizeInch { get; set; }
        public decimal? FuelTankCapacityLiter { get; set; }
        public int? TrimLevel { get; set; }
        public int? VehicleClass { get; set; }
        public string? PlateNumberAr { get; set; }
        public string? PlateNumberEn { get; set; }
        public int? TransmisionType { get; set; }
        public int? Drivetrain { get; set; }
        public int? Cylenders { get; set; }
        public int? FuelType { get; set; }
        public int? ManufactureCountryId { get; set; }
        public string? EnginNumber { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsAvailable { get; set; }
    }
}
