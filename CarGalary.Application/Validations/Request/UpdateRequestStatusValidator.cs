using CarGalary.Application.Dtos.Request.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Request
{
    public class UpdateRequestStatusValidator : AbstractValidator<UpdateRequestStatusDto>
    {
        public UpdateRequestStatusValidator()
        {
            RuleFor(x => x.CurrentStatus)
                .GreaterThan(0).WithMessage("CurrentStatus is required.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000);
        }
    }
}
