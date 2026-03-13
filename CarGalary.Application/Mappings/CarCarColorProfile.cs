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
            CreateMap<CarColor, CarCarColorResponseDto>()
                .ForMember(dest => dest.ColorNameAr, opt => opt.MapFrom(src => src.Color != null ? src.Color.ColorNameAr : null))
                .ForMember(dest => dest.ColorNameEn, opt => opt.MapFrom(src => src.Color != null ? src.Color.ColorNameEn : null))
                .ForMember(dest => dest.ColorCode, opt => opt.MapFrom(src => src.Color != null ? src.Color.ColorCode : null))
                .ForMember(dest => dest.ColorStatus, opt => opt.MapFrom(src => src.ColorStatus))
                .ForMember(dest => dest.ColorStatusDetailCode, opt => opt.MapFrom(src => src.ColorStatusLookup != null ? src.ColorStatusLookup.DetailCode : null))
                .ForMember(dest => dest.ColorStatusNameAr, opt => opt.MapFrom(src => src.ColorStatusLookup != null ? src.ColorStatusLookup.NameAr : null))
                .ForMember(dest => dest.ColorStatusNameEn, opt => opt.MapFrom(src => src.ColorStatusLookup != null ? src.ColorStatusLookup.NameEn : null));
            CreateMap<CreateCarCarColorRequestDto, CarColor>();
            CreateMap<UpdateCarCarColorRequestDto, CarColor>();
        }
    }
}
