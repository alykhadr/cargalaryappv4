using CarGalary.Application.Dtos.CarType.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.CarType
{
    public class UpdateCarTypeRequestValidator : AbstractValidator<UpdateCarTypeRequestDto>
    {
        public UpdateCarTypeRequestValidator()
        {
            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("Type English name is required")
                .MaximumLength(100);

            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Type Arabic name is required")
                .MaximumLength(100);
        }
    }
}
