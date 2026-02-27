using CarGalary.Application.Dtos.Department.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Department
{
    public class UpdateDepartmentRequestValidator : AbstractValidator<UpdateDepartmentRequestDto>
    {
        public UpdateDepartmentRequestValidator()
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
