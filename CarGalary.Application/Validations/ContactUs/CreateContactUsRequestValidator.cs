using CarGalary.Application.Dtos.ContactUs.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.ContactUs
{
    public class CreateContactUsRequestValidator : AbstractValidator<CreateContactUsRequestDto>
    {
        public CreateContactUsRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().WithMessage("NameAr is required").MaximumLength(100);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage("NameEn is required").MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Email is not valid");
            RuleFor(x => x.MobileNo).NotEmpty().WithMessage("MobileNo is required").MaximumLength(30);
            RuleFor(x => x.MessageAr).NotEmpty().WithMessage("MessageAr is required");
            RuleFor(x => x.MessageEn).NotEmpty().WithMessage("MessageEn is required");
        }
    }
}
