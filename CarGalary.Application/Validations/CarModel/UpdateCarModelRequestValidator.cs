using CarGalary.Application.Dtos.CarModel.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.CarModel
{
    public class UpdateCarModelRequestValidator : AbstractValidator<UpdateCarModelRequestDto>
    {
        public UpdateCarModelRequestValidator()
        {
            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("Model English name is required")
                .MaximumLength(100);

            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Model Arabic name is required")
                .MaximumLength(100);

            RuleFor(x => x.BrandId)
                .GreaterThan(0).WithMessage("BrandId is required");

            RuleFor(x => x.ImageFile)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Image size must be less than or equal to 5 MB");
        }
    }
}
