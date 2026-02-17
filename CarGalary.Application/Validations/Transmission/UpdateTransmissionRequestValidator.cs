using CarGalary.Application.Dtos.Transmission.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Transmission
{
    public class UpdateTransmissionRequestValidator : AbstractValidator<UpdateTransmissionRequestDto>
    {
        public UpdateTransmissionRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().WithMessage("NameAr is required").MaximumLength(100);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage("NameEn is required").MaximumLength(100);
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("CarId is required");
        }
    }
}
