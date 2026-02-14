using CarGalary.Application.Dtos.CarCarColor.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.CarCarColor
{
    public class CreateCarCarColorRequestValidator : AbstractValidator<CreateCarCarColorRequestDto>
    {
        public CreateCarCarColorRequestValidator()
        {
            RuleFor(x => x.CarId)
                .GreaterThan(0).WithMessage("CarId is required");

            RuleFor(x => x.ColorId)
                .GreaterThan(0).WithMessage("ColorId is required");

            RuleFor(x => x.StockQuantity)
                .GreaterThan(0).NotEmpty();
                 RuleFor(x => x.PricingPerColor)
                .GreaterThan(0).NotEmpty();

            RuleFor(x => x.ColorImageFile)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Image size must be less than or equal to 5 MB");
        }
    }
}
