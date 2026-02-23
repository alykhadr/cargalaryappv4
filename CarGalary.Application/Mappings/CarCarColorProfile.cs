using AutoMapper;
using CarGalary.Application.Dtos.CarCarColor.Command;
using CarGalary.Application.Dtos.CarCarColor.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class CarCarColorProfile : Profile
    {
        public CarCarColorProfile()
        {
            CreateMap<CarColor, CarCarColorResponseDto>();
            CreateMap<CreateCarCarColorRequestDto, CarColor>();
            CreateMap<UpdateCarCarColorRequestDto, CarColor>();
        }
    }
}

