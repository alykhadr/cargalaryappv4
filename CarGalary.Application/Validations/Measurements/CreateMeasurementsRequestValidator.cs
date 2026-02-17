using CarGalary.Application.Dtos.Measurements.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Measurements
{
    public class CreateMeasurementsRequestValidator : AbstractValidator<CreateMeasurementsRequestDto>
    {
        public CreateMeasurementsRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().WithMessage("NameAr is required").MaximumLength(100);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage("NameEn is required").MaximumLength(100);
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("CarId is required");
        }
    }
}
