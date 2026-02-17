using AutoMapper;
using CarGalary.Application.Dtos.MemberService.Command;
using CarGalary.Application.Dtos.MemberService.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class MemberServiceProfile : Profile
    {
        public MemberServiceProfile()
        {
            CreateMap<MemberService, MemberServiceResponseDto>();
            CreateMap<CreateMemberServiceRequestDto, MemberService>();
            CreateMap<UpdateMemberServiceRequestDto, MemberService>();
        }
    }
}
