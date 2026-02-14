
using AutoMapper;
using CarGalary.Application.Dtos.Brand;
using CarGalary.Application.Dtos.Brand.Command;
using CarGalary.Application.Dtos.Brand.Query;

namespace CarGalary.Application.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<CarBrand, BrandDto>();

            CreateMap<CarBrand, BrandResponseDto>();

            CreateMap<BrandDto, CarBrand>();
            CreateMap<CreateBrandRequestDto, CarBrand>();
            CreateMap<UpdateBrandRequestDto, CarBrand>();

        }
    }
}
