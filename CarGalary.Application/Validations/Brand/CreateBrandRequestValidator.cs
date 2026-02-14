using CarGalary.Application.Dtos.Brand.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Brand
{
    public class CreateBrandRequestValidator : AbstractValidator<CreateBrandRequestDto>
    {
        public CreateBrandRequestValidator()
        {
            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("Brand English name is required")
                .MaximumLength(100);

            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Brand Arabic name is required")
                .MaximumLength(100);

            RuleFor(x => x.ImageFile)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Image size must be less than or equal to 5 MB");
        }
    }
}
