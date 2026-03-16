using CarGalary.Application.Dtos.Package.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Package
{
    public class UpdatePackageRequestValidator : AbstractValidator<UpdatePackageRequestDto>
    {
        public UpdatePackageRequestValidator()
        {
            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage("Package English name is required")
                .MaximumLength(100);

            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Package Arabic name is required")
                .MaximumLength(100);

            RuleFor(x => x.ImageFile)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Image size must be less than or equal to 5 MB");
        }
    }
}
