using CarGalary.Application.Dtos.EngineSpecification.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.EngineSpecification
{
    public class CreateEngineSpecificationRequestValidator : AbstractValidator<CreateEngineSpecificationRequestDto>
    {
        public CreateEngineSpecificationRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().WithMessage("NameAr is required").MaximumLength(100);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage("NameEn is required").MaximumLength(100);
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("CarId is required");
        }
    }
}
