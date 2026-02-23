using CarGalary.Application.Dtos.ContactSalesOfficer.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.ContactSalesOfficer
{
    public class UpdateContactSalesOfficerRequestValidator : AbstractValidator<UpdateContactSalesOfficerRequestDto>
    {
        public UpdateContactSalesOfficerRequestValidator()
        {
            RuleFor(x => x.ContactValue)
                .NotEmpty().WithMessage("ContactValue is required")
                .MaximumLength(100);
            RuleFor(x => x.ContactType)
                .NotEmpty().WithMessage("ContactType is required");
        }
    }
}
