using CarGalary.Application.Dtos.ExtraFeature.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.ExtraFeature
{
    public class CreateExtraFeatureRequestValidator : AbstractValidator<CreateExtraFeatureRequestDto>
    {
        public CreateExtraFeatureRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().WithMessage("NameAr is required").MaximumLength(100);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage("NameEn is required").MaximumLength(100);
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("CarId is required");
        }
    }
}
