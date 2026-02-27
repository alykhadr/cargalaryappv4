using CarGalary.Application.Dtos.Quotation.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Quotation
{
    public class CreateQuotationRequestValidator : AbstractValidator<CreateQuotationRequestDto>
    {
        public CreateQuotationRequestValidator()
        {
            RuleFor(x => x.VehicleOwnerType)
                .GreaterThan(0).WithMessage("VehicleOwnerType is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is invalid.")
                .MaximumLength(256);

            RuleFor(x => x.MobileNo)
                .NotEmpty().WithMessage("MobileNo is required.")
                .MaximumLength(20);

            RuleFor(x => x.CarId)
                .GreaterThan(0).WithMessage("CarId is required.");

            RuleFor(x => x.PaymentMethod)
                .GreaterThan(0).WithMessage("PaymentMethod is required.");

            RuleFor(x => x.RegionId)
                .GreaterThan(0).WithMessage("RegionId is required.");

            RuleFor(x => x.CityId)
                .GreaterThan(0).WithMessage("CityId is required.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000);
        }
    }
}
