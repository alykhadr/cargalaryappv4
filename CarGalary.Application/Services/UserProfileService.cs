using AutoMapper;
using CarGalary.Application.Dtos.UserProfileAdmin.Command;
using CarGalary.Application.Dtos.UserProfileAdmin.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUnitOfWork _unitOfWork; private readonly IMapper _mapper;
        public UserProfileService(IUnitOfWork unitOfWork,IMapper mapper){_unitOfWork=unitOfWork;_mapper=mapper;}
        public async Task<List<UserProfileAdminResponseDto>> GetAllAsync(){var i=await _unitOfWork.UserProfiles.GetAllAsync(); return _mapper.Map<List<UserProfileAdminResponseDto>>(i);} 
        public async Task<UserProfileAdminResponseDto?> GetByIdAsync(int id){var i=await _unitOfWork.UserProfiles.GetByIdAsync(id); return i==null?null:_mapper.Map<UserProfileAdminResponseDto>(i);} 
        public async Task<UserProfileAdminResponseDto> CreateAsync(CreateUserProfileAdminRequestDto dto){var e=_mapper.Map<UserProfile>(dto); e.CreatedAt=DateTime.UtcNow; await _unitOfWork.UserProfiles.CreateProfileAsync(e); await _unitOfWork.SaveChangesAsync(); return _mapper.Map<UserProfileAdminResponseDto>(e);} 
        public async Task UpdateAsync(int id, UpdateUserProfileAdminRequestDto dto){var e=await _unitOfWork.UserProfiles.GetByIdAsync(id); if(e==null) throw new Exception("UserProfile not found"); if(dto.IsAvailable==null) dto.IsAvailable=e.IsAvailable; _mapper.Map(dto,e); await _unitOfWork.UserProfiles.UpdateProfileAsync(e); await _unitOfWork.SaveChangesAsync();}
        public async Task DeleteAsync(int id){var e=await _unitOfWork.UserProfiles.GetByIdAsync(id); if(e==null) throw new Exception("UserProfile not found"); await _unitOfWork.UserProfiles.DeleteProfileAsync(e); await _unitOfWork.SaveChangesAsync();}
    }
}
