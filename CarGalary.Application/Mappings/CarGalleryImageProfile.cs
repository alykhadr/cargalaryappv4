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
            CreateMap<CreateCarGalleryImageRequestDto, CarGalleryImage>();
            CreateMap<UpdateCarGalleryImageRequestDto, CarGalleryImage>();
        }
    }
}
