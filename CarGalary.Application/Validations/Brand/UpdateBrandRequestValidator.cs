using CarGalary.Application.Dtos.Brand.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Brand
{
    public class UpdateBrandRequestValidator : AbstractValidator<UpdateBrandRequestDto>
    {
        public UpdateBrandRequestValidator()
        {
            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("Brand English name is required")
                .MaximumLength(100);

            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Brand Arabic name is required")
                .MaximumLength(100);
        }
    }
}
