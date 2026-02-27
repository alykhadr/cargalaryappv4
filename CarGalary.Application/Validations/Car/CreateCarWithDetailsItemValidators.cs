using CarGalary.Application.Dtos.Car.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Car
{
    public class CreateCarWithDetailsFeatureItemDtoValidator : AbstractValidator<CreateCarWithDetailsFeatureItemDto>
    {
        public CreateCarWithDetailsFeatureItemDtoValidator()
        {
            RuleFor(x => x.FeatureId)
                .GreaterThan(0).WithMessage("FeatureId is required");
        }
    }

    public class CreateCarWithDetailsColorItemDtoValidator : AbstractValidator<CreateCarWithDetailsColorItemDto>
    {
        public CreateCarWithDetailsColorItemDtoValidator()
        {
            RuleFor(x => x.ColorId)
                .GreaterThan(0).WithMessage("ColorId is required");

            RuleFor(x => x.StockQuantity)
                .NotNull().WithMessage("StockQuantity is required")
                .GreaterThanOrEqualTo(0).WithMessage("StockQuantity must be zero or greater");

            RuleFor(x => x.PricingPerColor)
                .NotNull().WithMessage("PricingPerColor is required")
                .GreaterThanOrEqualTo(0).WithMessage("PricingPerColor must be zero or greater");

            RuleFor(x => x.PricePefore)
                .GreaterThanOrEqualTo(0).When(x => x.PricePefore.HasValue)
                .WithMessage("PricePefore must be zero or greater");

            RuleFor(x => x.Discount)
                .NotNull().WithMessage("Discount is required")
                .GreaterThanOrEqualTo(0).WithMessage("Discount must be zero or greater");

            RuleFor(x => x.DiscountType)
                .NotNull().WithMessage("DiscountType is required")
                .InclusiveBetween(CarGalary.Domain.Entities.CarColor.DiscountTypePercentage, CarGalary.Domain.Entities.CarColor.DiscountTypeFixedAmount)
                .WithMessage("DiscountType must be 0 (percentage) or 1 (fixed amount)");
        }
    }

    public class CreateCarWithDetailsExtraDetailItemDtoValidator : AbstractValidator<CreateCarWithDetailsExtraDetailItemDto>
    {
        public CreateCarWithDetailsExtraDetailItemDtoValidator()
        {
            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("English name is required")
                .MaximumLength(150);

            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Arabic name is required")
                .MaximumLength(150);

            RuleFor(x => x.CarExtraDetailsType)
                .GreaterThan(0).WithMessage("CarExtraDetailsType is required");

            RuleFor(x => x.DescriptionEn)
                .MaximumLength(2000);

            RuleFor(x => x.DescriptionAr)
                .MaximumLength(2000);
        }
    }

    public class CreateCarWithDetailsGalleryImageMetaItemDtoValidator : AbstractValidator<CreateCarWithDetailsGalleryImageMetaItemDto>
    {
        public CreateCarWithDetailsGalleryImageMetaItemDtoValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("FileName is required");

            RuleFor(x => x.ImageType)
                .NotNull().WithMessage("ImageType is required");

            RuleFor(x => x.ImageType)
                .GreaterThan(0)
                .When(x => x.ImageType.HasValue)
                .WithMessage("ImageType must be greater than 0");
        }
    }

    public class CreateCarWithDetailsCarColorImageMetaItemDtoValidator : AbstractValidator<CreateCarWithDetailsCarColorImageMetaItemDto>
    {
        public CreateCarWithDetailsCarColorImageMetaItemDtoValidator()
        {
            RuleFor(x => x.ColorId)
                .GreaterThan(0).WithMessage("ColorId is required");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("FileName is required");
        }
    }
}
