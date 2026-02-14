using CarGalary.Application.Dtos.CarColor.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.CarColor
{
    public class CreateCarColorRequestValidator : AbstractValidator<CreateCarColorRequestDto>
    {
        public CreateCarColorRequestValidator()
        {
            RuleFor(x => x.ColorNameEn)
                .NotEmpty().WithMessage("Color English name is required")
                .MaximumLength(100);

            RuleFor(x => x.ColorNameAr)
                .NotEmpty().WithMessage("Color Arabic name is required")
                .MaximumLength(100);

            RuleFor(x => x.ColorCode)
                .NotEmpty().WithMessage("Color code is required")
                .MaximumLength(30);
        }
    }
}

