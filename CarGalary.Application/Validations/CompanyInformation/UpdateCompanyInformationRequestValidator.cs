using CarGalary.Application.Dtos.CompanyInformation.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.CompanyInformation
{
    public class UpdateCompanyInformationRequestValidator : AbstractValidator<UpdateCompanyInformationRequestDto>
    {
        public UpdateCompanyInformationRequestValidator()
        {
            RuleFor(x => x.CompanyNameAr).NotEmpty().WithMessage("CompanyNameAr is required").MaximumLength(200);
            RuleFor(x => x.CompanyNameEn).NotEmpty().WithMessage("CompanyNameEn is required").MaximumLength(200);
            RuleFor(x => x.CRNumber).NotEmpty().WithMessage("CRNumber is required").MaximumLength(50);
            RuleFor(x => x.LogoUrl).NotEmpty().WithMessage("LogoUrl is required");
            RuleFor(x => x.MobileNo).NotEmpty().WithMessage("MobileNo is required")
                .Matches(@"^05\d{8}$").WithMessage("Mobile number must start with 05 and be 10 digits");
            RuleFor(x => x.TelNo).NotEmpty().WithMessage("TelNo is required").MaximumLength(30);
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Email is not valid");
            RuleFor(x => x.AboutUsAr).NotEmpty().WithMessage("AboutUsAr is required");
            RuleFor(x => x.AboutUsEn).NotEmpty().WithMessage("AboutUsEn is required");
            RuleFor(x => x.OurMissionAr).NotEmpty().WithMessage("OurMissionAr is required");
            RuleFor(x => x.OurMissionEn).NotEmpty().WithMessage("OurMissionEn is required");
            RuleFor(x => x.OurGoalsAr).NotEmpty().WithMessage("OurGoalsAr is required");
            RuleFor(x => x.OurGoalsEn).NotEmpty().WithMessage("OurGoalsEn is required");
            RuleFor(x => x.LogoFile)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Logo image size must be less than or equal to 5 MB");
        }
    }
}
