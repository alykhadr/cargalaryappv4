using AutoMapper;
using CarGalary.Application.Dtos.ServiceType.Command;
using CarGalary.Application.Dtos.ServiceType.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class ServiceTypeProfile : Profile
    {
        public ServiceTypeProfile(){CreateMap<ServiceType,ServiceTypeResponseDto>();CreateMap<CreateServiceTypeRequestDto,ServiceType>();CreateMap<UpdateServiceTypeRequestDto,ServiceType>();}
    }
}
