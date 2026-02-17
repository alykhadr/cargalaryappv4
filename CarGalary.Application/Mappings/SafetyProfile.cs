using AutoMapper;
using CarGalary.Application.Dtos.Safety.Command;
using CarGalary.Application.Dtos.Safety.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class SafetyProfile : Profile
    {
        public SafetyProfile()
        {
            CreateMap<Safety, SafetyResponseDto>();
            CreateMap<CreateSafetyRequestDto, Safety>();
            CreateMap<UpdateSafetyRequestDto, Safety>();
        }
    }
}
