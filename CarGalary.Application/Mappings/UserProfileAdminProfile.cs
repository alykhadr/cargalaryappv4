using AutoMapper;
using CarGalary.Application.Dtos.UserProfileAdmin.Command;
using CarGalary.Application.Dtos.UserProfileAdmin.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class UserProfileAdminProfile : Profile
    {
        public UserProfileAdminProfile(){CreateMap<UserProfile,UserProfileAdminResponseDto>();CreateMap<CreateUserProfileAdminRequestDto,UserProfile>();CreateMap<UpdateUserProfileAdminRequestDto,UserProfile>();}
    }
}
