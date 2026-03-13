using AutoMapper;
using CarGalary.Application.Dtos.Request.Command;
using CarGalary.Application.Dtos.Request.Query;
using CarGalary.Domain.Entities;
using System.Linq;

namespace CarGalary.Application.Mappings
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<CreateRequestDto, Request>();
            CreateMap<Request, RequestResponseDto>()
                .ForMember(dest => dest.ColorNameAr, opt => opt.MapFrom(src => src.Color != null ? src.Color.ColorNameAr : null))
                .ForMember(dest => dest.ColorNameEn, opt => opt.MapFrom(src => src.Color != null ? src.Color.ColorNameEn : null))
                .ForMember(dest => dest.ColorCode, opt => opt.MapFrom(src => src.Color != null ? src.Color.ColorCode : null))
                .ForMember(dest => dest.ColorStatus, opt => opt.MapFrom(src =>
                    src.Car != null
                        ? src.Car.CarColors.Where(cc => cc.ColorId == src.ColorId).Select(cc => (int?)cc.ColorStatus).FirstOrDefault()
                        : null))
                .ForMember(dest => dest.ColorStatusNameAr, opt => opt.MapFrom(src =>
                    src.Car != null
                        ? src.Car.CarColors.Where(cc => cc.ColorId == src.ColorId).Select(cc => cc.ColorStatusLookup != null ? cc.ColorStatusLookup.NameAr : null).FirstOrDefault()
                        : null))
                .ForMember(dest => dest.ColorStatusNameEn, opt => opt.MapFrom(src =>
                    src.Car != null
                        ? src.Car.CarColors.Where(cc => cc.ColorId == src.ColorId).Select(cc => cc.ColorStatusLookup != null ? cc.ColorStatusLookup.NameEn : null).FirstOrDefault()
                        : null))
                .ForMember(dest => dest.ColorStatusDetailCode, opt => opt.MapFrom(src =>
                    src.Car != null
                        ? src.Car.CarColors.Where(cc => cc.ColorId == src.ColorId).Select(cc => cc.ColorStatusLookup != null ? cc.ColorStatusLookup.DetailCode : null).FirstOrDefault()
                        : null))
                .ForMember(dest => dest.CurrentStatusNameAr, opt => opt.MapFrom(src => src.CurrentStatusLookup != null ? src.CurrentStatusLookup.NameAr : null))
                .ForMember(dest => dest.CurrentStatusNameEn, opt => opt.MapFrom(src => src.CurrentStatusLookup != null ? src.CurrentStatusLookup.NameEn : null));
            CreateMap<RequestHistory, RequestHistoryResponseDto>();
        }
    }
}
