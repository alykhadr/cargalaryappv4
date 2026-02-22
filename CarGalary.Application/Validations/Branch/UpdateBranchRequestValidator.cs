using CarGalary.Application.Dtos.Branch.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Branch
{
    public class UpdateBranchRequestValidator : AbstractValidator<UpdateBranchRequestDto>
    {
        public UpdateBranchRequestValidator()
        {
            RuleFor(x => x.BranchNameEn)
                .NotEmpty().WithMessage("Branch name (EN) is required")
                .MaximumLength(100);

            RuleFor(x => x.BranchNameAr)
                .NotEmpty().WithMessage("Branch name (AR) is required")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.MobileNo)
                .Matches(@"^05\d{8}$")
                .WithMessage("Mobile number must start with 05 and be 10 digits long")
                .When(x => !string.IsNullOrWhiteSpace(x.MobileNo));
        }
    }
}
