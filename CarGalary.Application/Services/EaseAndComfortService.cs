using AutoMapper;
using CarGalary.Application.Dtos.EaseAndComfort.Command;
using CarGalary.Application.Dtos.EaseAndComfort.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Services
{
    public class EaseAndComfortService : IEaseAndComfortService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public EaseAndComfortService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<EaseAndComfortResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.EaseAndComforts.GetAllAsync();
            return _mapper.Map<List<EaseAndComfortResponseDto>>(items);
        }

        public async Task<EaseAndComfortResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.EaseAndComforts.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<EaseAndComfortResponseDto>(item);
        }

        public async Task<EaseAndComfortResponseDto> CreateAsync(CreateEaseAndComfortRequestDto dto)
        {
            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            var entity = _mapper.Map<EaseAndComfort>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.EaseAndComforts.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<EaseAndComfortResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateEaseAndComfortRequestDto dto)
        {
            var existing = await _unitOfWork.EaseAndComforts.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("EaseAndComfort not found");
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
            await _unitOfWork.EaseAndComforts.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.EaseAndComforts.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("EaseAndComfort not found");
            }

            await _unitOfWork.EaseAndComforts.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
