using CarGalary.Application.Dtos.MemberService.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.MemberService
{
    public class CreateMemberServiceRequestValidator : AbstractValidator<CreateMemberServiceRequestDto>
    {
        public CreateMemberServiceRequestValidator(){RuleFor(x=>x.NameAr).NotEmpty(); RuleFor(x=>x.NameEn).NotEmpty();}
    }
}
