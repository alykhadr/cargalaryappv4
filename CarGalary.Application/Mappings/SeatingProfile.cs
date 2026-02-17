using AutoMapper;
using CarGalary.Application.Dtos.Seating.Command;
using CarGalary.Application.Dtos.Seating.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class SeatingProfile : Profile
    {
        public SeatingProfile()
        {
            CreateMap<Seating, SeatingResponseDto>();
            CreateMap<CreateSeatingRequestDto, Seating>();
            CreateMap<UpdateSeatingRequestDto, Seating>();
        }
    }
}
