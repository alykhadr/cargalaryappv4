using CarGalary.Application.Dtos.Safety.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Safety
{
    public class UpdateSafetyRequestValidator : AbstractValidator<UpdateSafetyRequestDto>
    {
        public UpdateSafetyRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().WithMessage("NameAr is required").MaximumLength(100);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage("NameEn is required").MaximumLength(100);
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("CarId is required");
        }
    }
}
