using AutoMapper;
using CarGalary.Application.Dtos.Request.Command;
using CarGalary.Application.Dtos.Request.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<CreateRequestDto, Request>();
            CreateMap<Request, RequestResponseDto>()
                .ForMember(dest => dest.CurrentStatusNameAr, opt => opt.MapFrom(src => src.CurrentStatusLookup != null ? src.CurrentStatusLookup.NameAr : null))
                .ForMember(dest => dest.CurrentStatusNameEn, opt => opt.MapFrom(src => src.CurrentStatusLookup != null ? src.CurrentStatusLookup.NameEn : null));
            CreateMap<RequestHistory, RequestHistoryResponseDto>();
        }
    }
}
