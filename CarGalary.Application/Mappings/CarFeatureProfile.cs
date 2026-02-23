using AutoMapper;
using CarGalary.Application.Dtos.CarFeature.Command;
using CarGalary.Application.Dtos.CarFeature.Query;

namespace CarGalary.Application.Mappings
{
    public class CarFeatureProfile : Profile
    {
        public CarFeatureProfile()
        {
            CreateMap<Feature, CarFeatureResponseDto>();
            CreateMap<CreateCarFeatureRequestDto, Feature>();
            CreateMap<UpdateCarFeatureRequestDto, Feature>();
        }
    }
}
