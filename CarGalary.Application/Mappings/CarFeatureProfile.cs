using AutoMapper;
using CarGalary.Application.Dtos.CarFeature.Command;
using CarGalary.Application.Dtos.CarFeature.Query;

namespace CarGalary.Application.Mappings
{
    public class CarFeatureProfile : Profile
    {
        public CarFeatureProfile()
        {
            CreateMap<CarFeature, CarFeatureResponseDto>();
            CreateMap<CreateCarFeatureRequestDto, CarFeature>();
            CreateMap<UpdateCarFeatureRequestDto, CarFeature>();
        }
    }
}
