using AutoMapper;
using CarGalary.Application.Dtos.Invoice.Command;
using CarGalary.Application.Dtos.Invoice.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<CreateInvoiceRequestDto, Invoice>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details));

            CreateMap<CreateInvoiceDetailRequestDto, InvoiceDetail>();

            CreateMap<UpdateInvoiceRequestDto, Invoice>()
                .ForMember(dest => dest.Details, opt => opt.Ignore());

            CreateMap<UpdateInvoiceDetailRequestDto, InvoiceDetail>();

            CreateMap<Invoice, InvoiceResponseDto>()
                .ForMember(dest => dest.UserFullNameAr, opt => opt.MapFrom(src => src.User != null ? src.User.FullNameAr : null))
                .ForMember(dest => dest.UserFullNameEn, opt => opt.MapFrom(src => src.User != null ? src.User.FullNameEn : null))
                .ForMember(dest => dest.UserPhoneNumber, opt => opt.MapFrom(src => src.User != null ? src.User.PhoneNumber : null))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
                .ForMember(dest => dest.BranchNameAr, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchNameAr : null))
                .ForMember(dest => dest.BranchNameEn, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchNameEn : null))
                .ForMember(dest => dest.PaymentMethodNameAr, opt => opt.MapFrom(src => src.PaymentMethodLookup != null ? src.PaymentMethodLookup.NameAr : null))
                .ForMember(dest => dest.PaymentMethodNameEn, opt => opt.MapFrom(src => src.PaymentMethodLookup != null ? src.PaymentMethodLookup.NameEn : null));

            CreateMap<InvoiceDetail, InvoiceDetailResponseDto>()
                .ForMember(dest => dest.CarNameAr, opt => opt.MapFrom(src => src.Car != null ? src.Car.NameAr : null))
                .ForMember(dest => dest.CarNameEn, opt => opt.MapFrom(src => src.Car != null ? src.Car.NameEn : null))
                .ForMember(dest => dest.ModelNameAr, opt => opt.MapFrom(src => src.Car != null && src.Car.CarModel != null ? src.Car.CarModel.NameAr : null))
                .ForMember(dest => dest.ModelNameEn, opt => opt.MapFrom(src => src.Car != null && src.Car.CarModel != null ? src.Car.CarModel.NameEn : null))
                .ForMember(dest => dest.BrandNameAr, opt => opt.MapFrom(src => src.Car != null && src.Car.CarModel != null && src.Car.CarModel.Brand != null ? src.Car.CarModel.Brand.NameAr : null))
                .ForMember(dest => dest.BrandNameEn, opt => opt.MapFrom(src => src.Car != null && src.Car.CarModel != null && src.Car.CarModel.Brand != null ? src.Car.CarModel.Brand.NameEn : null))
                .ForMember(dest => dest.PlateNumberAr, opt => opt.MapFrom(src => src.Car != null ? src.Car.PlateNumberAr : null))
                .ForMember(dest => dest.PlateNumberEn, opt => opt.MapFrom(src => src.Car != null ? src.Car.PlateNumberEn : null))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Car != null ? src.Car.Year : 0))
                .ForMember(dest => dest.Mileage, opt => opt.MapFrom(src => src.Car != null ? src.Car.Mileage : 0))
                .ForMember(dest => dest.PrimaryImageUrl, opt => opt.MapFrom(src => src.Car != null
                    ? src.Car.CarImages
                        .Where(i => i.IsAvailable && !string.IsNullOrWhiteSpace(i.ImageUrl))
                        .OrderByDescending(i => i.IsPrimary)
                        .ThenByDescending(i => i.CreatedAt)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                    : null))
                .ForMember(dest => dest.ColorNameAr, opt => opt.MapFrom(src => src.Color != null ? src.Color.ColorNameAr : null))
                .ForMember(dest => dest.ColorNameEn, opt => opt.MapFrom(src => src.Color != null ? src.Color.ColorNameEn : null))
                .ForMember(dest => dest.ColorCode, opt => opt.MapFrom(src => src.Color != null ? src.Color.ColorCode : null));
        }
    }
}
