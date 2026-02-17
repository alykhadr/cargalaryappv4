using AutoMapper;
using CarGalary.Application.Dtos.FAQ.Command;
using CarGalary.Application.Dtos.FAQ.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class FAQProfile : Profile
    {
        public FAQProfile()
        {
            CreateMap<FAQ, FAQResponseDto>();
            CreateMap<CreateFAQRequestDto, FAQ>();
            CreateMap<UpdateFAQRequestDto, FAQ>();
        }
    }
}
