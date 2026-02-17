using CarGalary.Application.Dtos.EaseAndComfort.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.EaseAndComfort
{
    public class CreateEaseAndComfortRequestValidator : AbstractValidator<CreateEaseAndComfortRequestDto>
    {
        public CreateEaseAndComfortRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().WithMessage("NameAr is required").MaximumLength(100);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage("NameEn is required").MaximumLength(100);
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("CarId is required");
        }
    }
}
