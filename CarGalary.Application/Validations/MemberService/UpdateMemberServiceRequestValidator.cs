using CarGalary.Application.Dtos.MemberService.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.MemberService
{
    public class UpdateMemberServiceRequestValidator : AbstractValidator<UpdateMemberServiceRequestDto>
    {
        public UpdateMemberServiceRequestValidator(){RuleFor(x=>x.NameAr).NotEmpty(); RuleFor(x=>x.NameEn).NotEmpty();}
    }
}
