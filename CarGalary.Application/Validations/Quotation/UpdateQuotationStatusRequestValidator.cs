using CarGalary.Application.Dtos.Quotation.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Quotation
{
    public class UpdateQuotationStatusRequestValidator : AbstractValidator<UpdateQuotationStatusRequestDto>
    {
        public UpdateQuotationStatusRequestValidator()
        {
            RuleFor(x => x.CurrentStatus)
                .GreaterThan(0).WithMessage("CurrentStatus is required.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000);
        }
    }
}
