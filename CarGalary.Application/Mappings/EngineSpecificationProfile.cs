using AutoMapper;
using CarGalary.Application.Dtos.EngineSpecification.Command;
using CarGalary.Application.Dtos.EngineSpecification.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class EngineSpecificationProfile : Profile
    {
        public EngineSpecificationProfile()
        {
            CreateMap<EngineSpecification, EngineSpecificationResponseDto>();
            CreateMap<CreateEngineSpecificationRequestDto, EngineSpecification>();
            CreateMap<UpdateEngineSpecificationRequestDto, EngineSpecification>();
        }
    }
}
