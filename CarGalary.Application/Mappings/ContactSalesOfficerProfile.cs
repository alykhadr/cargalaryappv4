using AutoMapper;
using CarGalary.Application.Dtos.ContactSalesOfficer.Command;
using CarGalary.Application.Dtos.ContactSalesOfficer.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class ContactSalesOfficerProfile : Profile
    {
        public ContactSalesOfficerProfile()
        {
            CreateMap<ContactSalesOfficer, ContactSalesOfficerResponseDto>();
            CreateMap<CreateContactSalesOfficerRequestDto, ContactSalesOfficer>();
            CreateMap<UpdateContactSalesOfficerRequestDto, ContactSalesOfficer>();
        }
    }
}
