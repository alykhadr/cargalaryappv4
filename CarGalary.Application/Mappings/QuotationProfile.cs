using AutoMapper;
using CarGalary.Application.Dtos.Quotation.Command;
using CarGalary.Application.Dtos.Quotation.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class QuotationProfile : Profile
    {
        public QuotationProfile()
        {
            CreateMap<CreateQuotationRequestDto, Quotation>();
            CreateMap<Quotation, QuotationResponseDto>();
        }
    }
}
