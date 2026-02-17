using AutoMapper;
using CarGalary.Application.Dtos.CompanyInformation.Command;
using CarGalary.Application.Dtos.CompanyInformation.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class CompanyInformationProfile : Profile
    {
        public CompanyInformationProfile()
        {
            CreateMap<CompanyInformation, CompanyInformationResponseDto>();
            CreateMap<CreateCompanyInformationRequestDto, CompanyInformation>();
            CreateMap<UpdateCompanyInformationRequestDto, CompanyInformation>();
        }
    }
}
