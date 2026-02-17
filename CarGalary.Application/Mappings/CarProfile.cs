using AutoMapper;
using CarGalary.Application.Dtos.Car.Command;
using CarGalary.Application.Dtos.Car.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<Car, CarResponseDto>();
            CreateMap<CreateCarRequestDto, Car>();
            CreateMap<UpdateCarRequestDto, Car>();
        }
    }
}
