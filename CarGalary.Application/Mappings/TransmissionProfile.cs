using AutoMapper;
using CarGalary.Application.Dtos.Transmission.Command;
using CarGalary.Application.Dtos.Transmission.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class TransmissionProfile : Profile
    {
        public TransmissionProfile()
        {
            CreateMap<Transmission, TransmissionResponseDto>();
            CreateMap<CreateTransmissionRequestDto, Transmission>();
            CreateMap<UpdateTransmissionRequestDto, Transmission>();
        }
    }
}
