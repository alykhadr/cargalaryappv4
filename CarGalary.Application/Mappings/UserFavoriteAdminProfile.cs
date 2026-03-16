using AutoMapper;
using CarGalary.Application.Dtos.UserFavoriteAdmin.Command;
using CarGalary.Application.Dtos.UserFavoriteAdmin.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class UserFavoriteAdminProfile : Profile
    {
        public UserFavoriteAdminProfile()
        {
            CreateMap<UserFavorite, UserFavoriteAdminResponseDto>()
                .ForMember(d => d.CarNameAr, o => o.MapFrom(s => s.Car != null ? s.Car.NameAr : null))
                .ForMember(d => d.CarNameEn, o => o.MapFrom(s => s.Car != null ? s.Car.NameEn : null))
                .ForMember(d => d.ModelId, o => o.MapFrom(s => s.Car != null ? (int?)s.Car.ModelId : null))
                .ForMember(d => d.ModelNameAr, o => o.MapFrom(s => s.Car != null && s.Car.CarModel != null ? s.Car.CarModel.NameAr : null))
                .ForMember(d => d.ModelNameEn, o => o.MapFrom(s => s.Car != null && s.Car.CarModel != null ? s.Car.CarModel.NameEn : null))
                .ForMember(d => d.BrandId, o => o.MapFrom(s => s.Car != null && s.Car.CarModel != null ? (int?)s.Car.CarModel.BrandId : null))
                .ForMember(d => d.BrandNameAr, o => o.MapFrom(s => s.Car != null && s.Car.CarModel != null && s.Car.CarModel.Brand != null ? s.Car.CarModel.Brand.NameAr : null))
                .ForMember(d => d.BrandNameEn, o => o.MapFrom(s => s.Car != null && s.Car.CarModel != null && s.Car.CarModel.Brand != null ? s.Car.CarModel.Brand.NameEn : null));

            CreateMap<CreateUserFavoriteAdminRequestDto, UserFavorite>();
            CreateMap<UpdateUserFavoriteAdminRequestDto, UserFavorite>();
        }
    }
}
