using CarGalary.Application.Dtos.Car.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Car
{
    public class CreateCarRequestValidator : AbstractValidator<CreateCarRequestDto>
    {
        public CreateCarRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().WithMessage("NameAr is required");
            RuleFor(x => x.NameEn).NotEmpty().WithMessage("NameEn is required");
            RuleFor(x => x.ModelId).GreaterThan(0).WithMessage("ModelId is required");
            RuleFor(x => x.TypeId).GreaterThan(0).WithMessage("TypeId is required");
            RuleFor(x => x.BranchId).GreaterThan(0).WithMessage("BranchId is required");
            RuleFor(x => x.Year).GreaterThan(1900).WithMessage("Year is required");
            RuleFor(x => x.Mileage).GreaterThanOrEqualTo(0).WithMessage("Mileage must be zero or greater");
            RuleFor(x => x.DescriptionAr).NotEmpty().WithMessage("DescriptionAr is required");
            RuleFor(x => x.DescriptionEn).NotEmpty().WithMessage("DescriptionEn is required");
        }
    }
}
