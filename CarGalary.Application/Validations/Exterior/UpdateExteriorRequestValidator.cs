using CarGalary.Application.Dtos.Exterior.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Exterior
{
    public class UpdateExteriorRequestValidator : AbstractValidator<UpdateExteriorRequestDto>
    {
        public UpdateExteriorRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().WithMessage("NameAr is required").MaximumLength(100);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage("NameEn is required").MaximumLength(100);
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("CarId is required");
        }
    }
}
