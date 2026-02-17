using AutoMapper;
using CarGalary.Application.Dtos.ExtraFeature.Command;
using CarGalary.Application.Dtos.ExtraFeature.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class ExtraFeatureProfile : Profile
    {
        public ExtraFeatureProfile()
        {
            CreateMap<ExtraFeature, ExtraFeatureResponseDto>();
            CreateMap<CreateExtraFeatureRequestDto, ExtraFeature>();
            CreateMap<UpdateExtraFeatureRequestDto, ExtraFeature>();
        }
    }
}
