using AutoMapper;
using CarGalary.Application.Dtos.PrivacyPolicy.Command;
using CarGalary.Application.Dtos.PrivacyPolicy.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class PrivacyPolicyProfile : Profile
    {
        public PrivacyPolicyProfile()
        {
            CreateMap<PrivacyPolicy, PrivacyPolicyResponseDto>();
            CreateMap<CreatePrivacyPolicyRequestDto, PrivacyPolicy>();
            CreateMap<UpdatePrivacyPolicyRequestDto, PrivacyPolicy>();
        }
    }
}
