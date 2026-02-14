using AutoMapper;
using CarGalary.Application.Dtos.CarColor.Command;
using CarGalary.Application.Dtos.CarColor.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class CarColorProfile : Profile
    {
        public CarColorProfile()
        {
            CreateMap<CarColor, CarColorResponseDto>();
            CreateMap<CreateCarColorRequestDto, CarColor>();
            CreateMap<UpdateCarColorRequestDto, CarColor>();
        }
    }
}

