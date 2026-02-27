using CarGalary.Application.Dtos.Department.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Department
{
    public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequestDto>
    {
        public CreateDepartmentRequestValidator()
        {
            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("Name (EN) is required")
                .MaximumLength(100);

            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Name (AR) is required")
                .MaximumLength(100);
        }
    }
}
