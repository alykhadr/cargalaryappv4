using AutoMapper;
using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Command;
using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Query;
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

