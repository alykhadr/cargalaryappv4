using AutoMapper;
using CarGalary.Application.Dtos.Package.Command;
using CarGalary.Application.Dtos.Package.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class PackageProfile : Profile
    {
        public PackageProfile()
        {
            CreateMap<Packages, PackageResponseDto>();
            CreateMap<CreatePackageRequestDto, Packages>();
            CreateMap<UpdatePackageRequestDto, Packages>();
        }
    }
}
