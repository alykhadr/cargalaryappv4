using AutoMapper;
using CarGalary.Application.Dtos.Department.Command;
using CarGalary.Application.Dtos.Department.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentRequestDto, Department>();
            CreateMap<UpdateDepartmentRequestDto, Department>();
            CreateMap<Department, DepartmentResponseDto>();
        }
    }
}
