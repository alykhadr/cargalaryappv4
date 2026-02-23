
using CarGalary.Application.Dtos.CarExtraDetails.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.AudioAndCommunicationSystem
{
    public class UpdateCarExtraDetailsRequestValidator : AbstractValidator<UpdateCarExtraDetailsRequestDto>
    {
        public UpdateCarExtraDetailsRequestValidator()
        {
            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("English name is required")
                .MaximumLength(150);

            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Arabic name is required")
                .MaximumLength(150);

            RuleFor(x => x.CarId)
                .GreaterThan(0).WithMessage("CarId is required");

            RuleFor(x => x.DescriptionEn)
                .MaximumLength(2000);

            RuleFor(x => x.DescriptionAr)
                .MaximumLength(2000);
        }
    }
}

