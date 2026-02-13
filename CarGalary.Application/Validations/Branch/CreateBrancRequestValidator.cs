
using CarGalary.Application.Dtos.Branch.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Branch
{

    public class CreateBrancRequestValidator : AbstractValidator<CreateBrancRequestDto>
    {
        public CreateBrancRequestValidator()
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
            .NotEmpty().WithMessage("Mobile number is required")
            .Matches(@"^05\d{8}$")
            .WithMessage("Mobile number must start with 05 and be 10 digits long");

        RuleFor(x => x.Address)
            .NotEmpty();

        RuleFor(x => x.Latitute)
            .NotEmpty();

        RuleFor(x => x.Longtute)
            .NotEmpty();

        // 🔴 LIST MUST HAVE AT LEAST ONE ITEM
        RuleFor(x => x.CreateBranchWorkingDaysRequestDto)
            .NotNull()
            .NotEmpty()
            .WithMessage("At least one working day is required");

        // 🔴 VALIDATE EACH ITEM IN LIST
        RuleForEach(x => x.CreateBranchWorkingDaysRequestDto)
            .SetValidator(new CreateBranchWorkingDaysRequestValidator());
        }
    }
}