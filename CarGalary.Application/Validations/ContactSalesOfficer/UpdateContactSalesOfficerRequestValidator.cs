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
            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("BranchId is required")
                .GreaterThan(0).WithMessage("BranchId must be greater than 0");
        }
    }
}
