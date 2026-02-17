using CarGalary.Application.Dtos.CarGalleryImage.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.CarGalleryImage
{
    public class UpdateCarGalleryImageRequestValidator : AbstractValidator<UpdateCarGalleryImageRequestDto>
    {
        public UpdateCarGalleryImageRequestValidator()
        {
            RuleFor(x => x.CarId)
                .GreaterThan(0).WithMessage("CarId is required");

            RuleFor(x => x.ImageType)
                .NotNull().WithMessage("ImageType is required");

            RuleFor(x => x.ImageFile)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Image size must be less than or equal to 5 MB");
        }
    }
}
