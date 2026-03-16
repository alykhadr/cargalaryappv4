using AutoMapper;
using CarGalary.Application.Dtos.Package.Command;
using CarGalary.Application.Dtos.Package.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public PackageService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<PackageResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.Packages.GetAllAsync();
            return _mapper.Map<List<PackageResponseDto>>(items);
        }

        public async Task<PackageResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.Packages.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<PackageResponseDto>(item);
        }

        public async Task<PackageResponseDto> CreateAsync(CreatePackageRequestDto dto)
        {
            var entity = _mapper.Map<Packages>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.Packages.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PackageResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdatePackageRequestDto dto)
        {
            var entity = await _unitOfWork.Packages.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception("Package not found");
            }

            if (dto.IsAvailable == null)
            {
                dto.IsAvailable = entity.IsAvailable;
            }

            if (string.IsNullOrWhiteSpace(dto.ImageUrl))
            {
                dto.ImageUrl = entity.ImageUrl;
            }

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUserService.UserName;

            await _unitOfWork.Packages.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Packages.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception("Package not found");
            }

            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUserService.UserName;

            await _unitOfWork.Packages.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
