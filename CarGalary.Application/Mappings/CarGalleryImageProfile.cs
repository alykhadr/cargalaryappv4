using AutoMapper;
using CarGalary.Application.Dtos.CarGalleryImage.Command;
using CarGalary.Application.Dtos.CarGalleryImage.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class CarGalleryImageProfile : Profile
    {
        public CarGalleryImageProfile()
        {
            CreateMap<CarGalleryImage, CarGalleryImageResponseDto>();
            
            CreateMap<CreateCarGalleryImageRequestDto, CarGalleryImage>()
                .ForMember(dest => dest.ImageType, opt => opt.MapFrom(src => src.ImageType))
                .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.IsPrimary));
            
            CreateMap<UpdateCarGalleryImageRequestDto, CarGalleryImage>()
                .ForMember(dest => dest.ImageType, opt => opt.MapFrom(src => src.ImageType))
                .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.IsPrimary))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null || opts.DestinationMember.Name == "IsPrimary" || opts.DestinationMember.Name == "ImageType"));
        }
    }
}
