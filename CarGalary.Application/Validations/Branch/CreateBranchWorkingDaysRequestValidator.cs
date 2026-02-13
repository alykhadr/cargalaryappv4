
using CarGalary.Application.Dtos.Branch.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Branch
{
    public class CreateBranchWorkingDaysRequestValidator : AbstractValidator<CreateBranchWorkingDaysRequestDto>
    {
        public CreateBranchWorkingDaysRequestValidator()
        {
            RuleFor(x => x.IsAvailable)
                 .NotNull()
                 .WithMessage("Availability is required");

            RuleFor(x => x.DayAr)
                .NotEmpty()
                .WithMessage("Arabic day name is required")
                .MaximumLength(20);

            RuleFor(x => x.DayEn)
                .NotEmpty()
                .WithMessage("English day name is required")
                .MaximumLength(20);

            RuleFor(x => x.TimeType)
                .NotEmpty()
                .Must(t => t == "AM" || t == "PM" || t == "24H")
                .WithMessage("TimeType must be AM, PM, or 24H");

            // 🔴 Working hours validation (only if available)
            When(x => x.IsAvailable == true, () =>
            {
                RuleFor(x => x.WorkingFrom)
                    .NotNull()
                    .InclusiveBetween(0, 23)
                    .WithMessage("WorkingFrom must be between 0 and 23");

                RuleFor(x => x.WorkingTo)
                    .NotNull()
                    .InclusiveBetween(0, 23)
                    .WithMessage("WorkingTo must be between 0 and 23");

                RuleFor(x => x)
                    .Must(x => x.WorkingFrom < x.WorkingTo)
                    .WithMessage("WorkingFrom must be less than WorkingTo");
            });

            // 🔴 If NOT available → no working hours allowed
            When(x => x.IsAvailable == false, () =>
            {
                RuleFor(x => x.WorkingFrom)
                    .Null()
                    .WithMessage("WorkingFrom must be null when day is not available");

                RuleFor(x => x.WorkingTo)
                    .Null()
                    .WithMessage("WorkingTo must be null when day is not available");
            });

        }
    }
}