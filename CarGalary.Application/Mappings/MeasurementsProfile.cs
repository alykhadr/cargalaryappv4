using AutoMapper;
using CarGalary.Application.Dtos.Measurements.Command;
using CarGalary.Application.Dtos.Measurements.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class MeasurementsProfile : Profile
    {
        public MeasurementsProfile()
        {
            CreateMap<Measurements, MeasurementsResponseDto>();
            CreateMap<CreateMeasurementsRequestDto, Measurements>();
            CreateMap<UpdateMeasurementsRequestDto, Measurements>();
        }
    }
}
