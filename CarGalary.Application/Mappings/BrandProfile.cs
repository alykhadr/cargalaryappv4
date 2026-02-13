
using AutoMapper;
using CarGalary.Application.Dtos.Brand;

namespace CarGalary.Application.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<CarBrand, BrandDto>();

             CreateMap<BrandDto, CarBrand>();

        }
    }
}