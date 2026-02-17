using AutoMapper;
using CarGalary.Application.Dtos.CarType.Command;
using CarGalary.Application.Dtos.CarType.Query;

namespace CarGalary.Application.Mappings
{
    public class CarTypeProfile : Profile
    {
        public CarTypeProfile()
        {
            CreateMap<CarType, CarTypeResponseDto>();
            CreateMap<CreateCarTypeRequestDto, CarType>();
            CreateMap<UpdateCarTypeRequestDto, CarType>();
        }
    }
}
