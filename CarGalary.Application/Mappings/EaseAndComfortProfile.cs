using AutoMapper;
using CarGalary.Application.Dtos.EaseAndComfort.Command;
using CarGalary.Application.Dtos.EaseAndComfort.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class EaseAndComfortProfile : Profile
    {
        public EaseAndComfortProfile()
        {
            CreateMap<EaseAndComfort, EaseAndComfortResponseDto>();
            CreateMap<CreateEaseAndComfortRequestDto, EaseAndComfort>();
            CreateMap<UpdateEaseAndComfortRequestDto, EaseAndComfort>();
        }
    }
}
