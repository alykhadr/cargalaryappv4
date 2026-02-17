using CarGalary.Application.Dtos.Services.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Services
{
    public class CreateServicesRequestValidator : AbstractValidator<CreateServicesRequestDto>
    {
        public CreateServicesRequestValidator(){RuleFor(x=>x.NameAr).NotEmpty(); RuleFor(x=>x.NameEn).NotEmpty(); RuleFor(x=>x.ServiceTypeId).GreaterThan(0);} 
    }
}
