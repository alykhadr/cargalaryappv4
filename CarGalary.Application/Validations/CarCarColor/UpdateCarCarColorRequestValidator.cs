using CarGalary.Application.Dtos.CarCarColor.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.CarCarColor
{
    public class UpdateCarCarColorRequestValidator : AbstractValidator<UpdateCarCarColorRequestDto>
    {
        public UpdateCarCarColorRequestValidator()
        {
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

            RuleFor(x => x.ColorImageFile)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Image size must be less than or equal to 5 MB");
        }
    }
}
