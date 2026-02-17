using AutoMapper;
using CarGalary.Application.Dtos.Safety.Command;
using CarGalary.Application.Dtos.Safety.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Services
{
    public class SafetyService : ISafetyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public SafetyService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<SafetyResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.Safeties.GetAllAsync();
            return _mapper.Map<List<SafetyResponseDto>>(items);
        }

        public async Task<SafetyResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.Safeties.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<SafetyResponseDto>(item);
        }

        public async Task<SafetyResponseDto> CreateAsync(CreateSafetyRequestDto dto)
        {
            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            var entity = _mapper.Map<Safety>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.Safeties.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SafetyResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateSafetyRequestDto dto)
        {
            var existing = await _unitOfWork.Safeties.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Safety not found");
            }

            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            if (dto.IsAvailable == null)
            {
                dto.IsAvailable = existing.IsAvailable;
            }

            _mapper.Map(dto, existing);
            await _unitOfWork.Safeties.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.Safeties.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Safety not found");
            }

            await _unitOfWork.Safeties.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
