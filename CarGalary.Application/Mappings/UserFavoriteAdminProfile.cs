using AutoMapper;
using CarGalary.Application.Dtos.UserFavoriteAdmin.Command;
using CarGalary.Application.Dtos.UserFavoriteAdmin.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class UserFavoriteAdminProfile : Profile
    {
        public UserFavoriteAdminProfile(){CreateMap<UserFavorite,UserFavoriteAdminResponseDto>();CreateMap<CreateUserFavoriteAdminRequestDto,UserFavorite>();CreateMap<UpdateUserFavoriteAdminRequestDto,UserFavorite>();}
    }
}
