using AutoMapper;
using CarGalary.Application.Dtos.ContactUs.Command;
using CarGalary.Application.Dtos.ContactUs.Query;

namespace CarGalary.Application.Mappings
{
    public class ContactUsProfile : Profile
    {
        public ContactUsProfile()
        {
            CreateMap<ContactUs, ContactUsResponseDto>();
            CreateMap<CreateContactUsRequestDto, ContactUs>();
            CreateMap<UpdateContactUsRequestDto, ContactUs>();
        }
    }
}
