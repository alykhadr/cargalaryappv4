using CarGalary.Application.Dtos.CarCarColor.Query;
using CarGalary.Application.Dtos.CarFeature.Query;

namespace CarGalary.Application.Dtos.Car.Query
{
    public class CarApiResponseDto
    {
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int ModelId { get; set; }
        public string? ModelNameAr { get; set; }
        public string? ModelNameEn { get; set; }
        public int? BrandId { get; set; }
        public string? BrandNameAr { get; set; }
        public string? BrandNameEn { get; set; }
        public int TypeId { get; set; }
        public string? TypeNameAr { get; set; }
        public string? TypeNameEn { get; set; }
        public int BranchId { get; set; }
        public string? BranchNameAr { get; set; }
        public string? BranchNameEn { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public decimal? Vat { get; set; }
        public int? ConditionId { get; set; }
        public string? ConditionNameAr { get; set; }
        public string? ConditionNameEn { get; set; }
        public int? SeatingCapacity { get; set; }
        public string? WeelSizeInch { get; set; }
        public decimal? FuelTankCapacityLiter { get; set; }
        public int? TrimLevel { get; set; }
        public string? TrimLevelNameAr { get; set; }
        public string? TrimLevelNameEn { get; set; }
        public int? VehicleClass { get; set; }
        public string? VehicleClassNameAr { get; set; }
        public string? VehicleClassNameEn { get; set; }
        public string? PlateNumberAr { get; set; }
        public string? PlateNumberEn { get; set; }
        public int? TransmisionType { get; set; }
        public string? TransmisionTypeNameAr { get; set; }
        public string? TransmisionTypeNameEn { get; set; }
        public int? Drivetrain { get; set; }
        public string? DrivetrainNameAr { get; set; }
        public string? DrivetrainNameEn { get; set; }
        public int? Cylenders { get; set; }
        public int? FuelType { get; set; }
        public string? FuelTypeNameAr { get; set; }
        public string? FuelTypeNameEn { get; set; }
        public int? ManufactureCountryId { get; set; }
        public string? ManufactureCountryNameAr { get; set; }
        public string? ManufactureCountryNameEn { get; set; }
        public string? EnginNumber { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsAvailable { get; set; }
        public List<CarFeatureResponseDto> Features { get; set; } = new();
        public List<CarCarColorResponseDto> Colors { get; set; } = new();
        public List<CarExtraDetailApiDto> ExtraDetails { get; set; } = new();
        public List<CarGalleryImageApiDto> GalleryImages { get; set; } = new();
    }

    public class CarExtraDetailApiDto
    {
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public int CarExtraDetailsType { get; set; }
        public string? CarExtraDetailsTypeNameAr { get; set; }
        public string? CarExtraDetailsTypeNameEn { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsAvailable { get; set; }
        public int CarId { get; set; }
    }

    public class CarGalleryImageApiDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string? ImageUrl { get; set; }
        public int? ImageType { get; set; }
        public string? ImageTypeNameAr { get; set; }
        public string? ImageTypeNameEn { get; set; }
        public bool IsPrimary { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsAvailable { get; set; }
    }
}
