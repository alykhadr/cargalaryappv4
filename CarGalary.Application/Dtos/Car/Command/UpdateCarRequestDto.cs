namespace CarGalary.Application.Dtos.Car.Command
{
    public class UpdateCarRequestDto
    {
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
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
