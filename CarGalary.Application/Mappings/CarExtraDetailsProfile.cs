using AutoMapper;
using CarGalary.Application.Dtos.CarExtraDetails.Query;
using CarGalary.Application.Dtos.CarExtraDetails.Command;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class CarExtraDetailsProfile : Profile
    {
        public CarExtraDetailsProfile()
        {
            CreateMap<CarExtraDetails, CarExtraDetailsResponseDto>();
            CreateMap<CreateCarExtraDetailsRequestDto, CarExtraDetails>();
            CreateMap<UpdateCarExtraDetailsRequestDto, CarExtraDetails>();
        }
    }
}

