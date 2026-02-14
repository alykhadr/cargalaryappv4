using AutoMapper;
using CarGalary.Application.Dtos.CarModel.Command;
using CarGalary.Application.Dtos.CarModel.Query;

namespace CarGalary.Application.Mappings
{
    public class CarModelProfile : Profile
    {
        public CarModelProfile()
        {
            CreateMap<CarGalary.Domain.Entities.CarModel, CarModelResponseDto>();
            CreateMap<CreateCarModelRequestDto, CarGalary.Domain.Entities.CarModel>();
            CreateMap<UpdateCarModelRequestDto, CarGalary.Domain.Entities.CarModel>();
        }
    }
}
