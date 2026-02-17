using CarGalary.Application.Dtos.Seating.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Seating
{
    public class CreateSeatingRequestValidator : AbstractValidator<CreateSeatingRequestDto>
    {
        public CreateSeatingRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().WithMessage("NameAr is required").MaximumLength(100);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage("NameEn is required").MaximumLength(100);
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("CarId is required");
        }
    }
}
