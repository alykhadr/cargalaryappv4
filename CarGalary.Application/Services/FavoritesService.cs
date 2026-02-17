using AutoMapper;
using CarGalary.Application.Dtos.UserFavoriteAdmin.Command;
using CarGalary.Application.Dtos.UserFavoriteAdmin.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly IUnitOfWork _unitOfWork; private readonly IMapper _mapper;
        public FavoritesService(IUnitOfWork unitOfWork,IMapper mapper){_unitOfWork=unitOfWork;_mapper=mapper;}
        public async Task<List<UserFavoriteAdminResponseDto>> GetAllAsync(){var i=await _unitOfWork.Favorites.GetAllAsync(); return _mapper.Map<List<UserFavoriteAdminResponseDto>>(i);} 
        public async Task<UserFavoriteAdminResponseDto?> GetByIdAsync(Guid userId,int carId){var i=await _unitOfWork.Favorites.GetByIdAsync(userId,carId); return i==null?null:_mapper.Map<UserFavoriteAdminResponseDto>(i);} 
        public async Task<UserFavoriteAdminResponseDto> CreateAsync(CreateUserFavoriteAdminRequestDto dto){var e=_mapper.Map<UserFavorite>(dto); e.CreatedAt=DateTime.UtcNow; await _unitOfWork.Favorites.CreateAsync(e); await _unitOfWork.SaveChangesAsync(); return _mapper.Map<UserFavoriteAdminResponseDto>(e);} 
        public async Task UpdateAsync(Guid userId,int carId,UpdateUserFavoriteAdminRequestDto dto){var e=await _unitOfWork.Favorites.GetByIdAsync(userId,carId); if(e==null) throw new Exception("UserFavorite not found"); _mapper.Map(dto,e); await _unitOfWork.Favorites.UpdateAsync(e); await _unitOfWork.SaveChangesAsync();}
        public async Task DeleteAsync(Guid userId,int carId){var e=await _unitOfWork.Favorites.GetByIdAsync(userId,carId); if(e==null) throw new Exception("UserFavorite not found"); await _unitOfWork.Favorites.DeleteAsync(e); await _unitOfWork.SaveChangesAsync();}
    }
}
