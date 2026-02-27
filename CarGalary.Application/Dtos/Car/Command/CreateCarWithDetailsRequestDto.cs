using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.Car.Command
{
    public class CreateCarWithDetailsRequestDto
    {
        // Car base fields
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
        public string? EnginNumber { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }

        // JSON payloads (multipart/form-data friendly)
        public string? FeaturesJson { get; set; }
        public string? CarColorsJson { get; set; }
        public string? ExtraDetailsJson { get; set; }
        public string? GalleryImagesMetaJson { get; set; }
        public string? CarColorImagesMetaJson { get; set; }

        // Files
        public List<IFormFile>? GalleryImageFiles { get; set; }
        public List<IFormFile>? CarColorImageFiles { get; set; }
    }

    public class CreateCarWithDetailsFeatureItemDto
    {
        public int FeatureId { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class CreateCarWithDetailsColorItemDto
    {
        public int ColorId { get; set; }
        public int? StockQuantity { get; set; }
        public string? ColorImageUrl { get; set; }
        public decimal? PricingPerColor { get; set; }
        public decimal? PricePefore { get; set; }
        public decimal? Discount { get; set; }
        public int? DiscountType { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class CreateCarWithDetailsExtraDetailItemDto
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public int CarExtraDetailsType { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class CreateCarWithDetailsGalleryImageMetaItemDto
    {
        public string? FileName { get; set; }
        public int? ImageType { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class CreateCarWithDetailsCarColorImageMetaItemDto
    {
        public int ColorId { get; set; }
        public string? FileName { get; set; }
    }
}
