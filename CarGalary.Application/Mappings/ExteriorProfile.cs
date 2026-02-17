using AutoMapper;
using CarGalary.Application.Dtos.Exterior.Command;
using CarGalary.Application.Dtos.Exterior.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class ExteriorProfile : Profile
    {
        public ExteriorProfile()
        {
            CreateMap<Exterior, ExteriorResponseDto>();
            CreateMap<CreateExteriorRequestDto, Exterior>();
            CreateMap<UpdateExteriorRequestDto, Exterior>();
        }
    }
}
