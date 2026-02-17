using AutoMapper;
using CarGalary.Application.Dtos.Measurements.Command;
using CarGalary.Application.Dtos.Measurements.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Services
{
    public class MeasurementsService : IMeasurementsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public MeasurementsService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<MeasurementsResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.Measurements.GetAllAsync();
            return _mapper.Map<List<MeasurementsResponseDto>>(items);
        }

        public async Task<MeasurementsResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.Measurements.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<MeasurementsResponseDto>(item);
        }

        public async Task<MeasurementsResponseDto> CreateAsync(CreateMeasurementsRequestDto dto)
        {
            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            var entity = _mapper.Map<Measurements>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.Measurements.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MeasurementsResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateMeasurementsRequestDto dto)
        {
            var existing = await _unitOfWork.Measurements.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Measurements not found");
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
            await _unitOfWork.Measurements.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.Measurements.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Measurements not found");
            }

            await _unitOfWork.Measurements.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
