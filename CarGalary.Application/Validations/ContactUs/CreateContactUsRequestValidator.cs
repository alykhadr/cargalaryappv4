using CarGalary.Application.Dtos.ContactUs.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.ContactUs
{
    public class CreateContactUsRequestValidator : AbstractValidator<CreateContactUsRequestDto>
    {
        public CreateContactUsRequestValidator()
        {
            RuleFor(x => x.ContactValue)
                .NotEmpty().WithMessage("ContactValue is required")
                .MaximumLength(100);
            RuleFor(x => x.ContactType)
                .NotEmpty().WithMessage("ContactType is required");
            RuleFor(x => x.MessageAr)
                .NotEmpty().WithMessage("MessageAr is required")
                .MaximumLength(500);
            RuleFor(x => x.MessageEn)
                .NotEmpty().WithMessage("MessageEn is required")
                .MaximumLength(500);
        }
    }
}
