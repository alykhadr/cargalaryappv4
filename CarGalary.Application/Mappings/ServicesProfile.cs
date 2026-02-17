using AutoMapper;
using CarGalary.Application.Dtos.Services.Command;
using CarGalary.Application.Dtos.Services.Query;

namespace CarGalary.Application.Mappings
{
    public class ServicesProfile : Profile
    {
        public ServicesProfile()
        {
            CreateMap<Domain.Entities.Services,ServicesResponseDto>();CreateMap<CreateServicesRequestDto,Domain.Entities.Services>();
            CreateMap<UpdateServicesRequestDto,Domain.Entities.Services>();}
    }
}
