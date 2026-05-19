using CarGalary.Application.Dtos.PrivacyPolicy.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.PrivacyPolicy
{
    public class CreatePrivacyPolicyRequestValidator : AbstractValidator<CreatePrivacyPolicyRequestDto>
    {
        public CreatePrivacyPolicyRequestValidator()
        {
            RuleFor(x => x.PrivacyPolicyAr).NotEmpty().WithMessage("PrivacyPolicyAr is required");
            RuleFor(x => x.PrivacyPolicyEn).NotEmpty().WithMessage("PrivacyPolicyEn is required");
        }
    }
}
