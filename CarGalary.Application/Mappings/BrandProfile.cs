
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
            CreateMap<Brand, BrandDto>();

            CreateMap<Brand, BrandResponseDto>();

            CreateMap<BrandDto, Brand>();
            CreateMap<CreateBrandRequestDto, Brand>();
            CreateMap<UpdateBrandRequestDto, Brand>();

        }
    }
}
