using AutoMapper;
using CarGalary.Application.Dtos.Offer.Command;
using CarGalary.Application.Dtos.Offer.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class OfferProfile : Profile
    {
        public OfferProfile(){CreateMap<Offer,OfferResponseDto>();CreateMap<CreateOfferRequestDto,Offer>();CreateMap<UpdateOfferRequestDto,Offer>();}
    }
}
