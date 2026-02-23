using CarGalary.Application.Dtos.MemberService.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.MemberService
{
    public class UpdateMemberServiceRequestValidator : AbstractValidator<UpdateMemberServiceRequestDto>
    {
        public UpdateMemberServiceRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().WithMessage("NameAr is required");
            RuleFor(x => x.NameEn).NotEmpty().WithMessage("NameEn is required");
        }
    }
}
