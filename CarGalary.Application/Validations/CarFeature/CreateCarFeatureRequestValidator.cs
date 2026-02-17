using CarGalary.Application.Dtos.CarFeature.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.CarFeature
{
    public class CreateCarFeatureRequestValidator : AbstractValidator<CreateCarFeatureRequestDto>
    {
        public CreateCarFeatureRequestValidator()
        {
            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("Feature English name is required")
                .MaximumLength(100);

            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Feature Arabic name is required")
                .MaximumLength(100);
        }
    }
}
