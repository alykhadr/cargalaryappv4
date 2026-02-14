using AutoMapper;
using CarGalary.Application.Dtos.CarCarColor.Command;
using CarGalary.Application.Dtos.CarCarColor.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class CarCarColorService : ICarCarColorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarCarColorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CarCarColorResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.CarCarColors.GetAllAsync();
            return _mapper.Map<List<CarCarColorResponseDto>>(items);
        }

        public async Task<CarCarColorResponseDto?> GetByIdAsync(int carId, int colorId)
        {
            var item = await _unitOfWork.CarCarColors.GetByIdAsync(carId, colorId);
            return item == null ? null : _mapper.Map<CarCarColorResponseDto>(item);
        }

        public async Task<List<CarCarColorResponseDto>> GetByCarIdAsync(int carId)
        {
            var items = await _unitOfWork.CarCarColors.GetByCarIdAsync(carId);
            return _mapper.Map<List<CarCarColorResponseDto>>(items);
        }

        public async Task<CarCarColorResponseDto> CreateAsync(CreateCarCarColorRequestDto dto)
        {
            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            var color = await _unitOfWork.CarColors.GetCarColorByIdAsync(dto.ColorId);
            if (color == null || !color.IsAvailable)
            {
                throw new Exception("CarColor not found");
            }

            var existing = await _unitOfWork.CarCarColors.GetByIdAsync(dto.CarId, dto.ColorId);
            if (existing != null)
            {
                throw new Exception("CarCarColor already exists");
            }

            var entity = _mapper.Map<CarCarColor>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsAvailable = true;

            await _unitOfWork.CarCarColors.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarCarColorResponseDto>(entity);
        }

        public async Task UpdateAsync(int carId, int colorId, UpdateCarCarColorRequestDto dto)
        {
            var existing = await _unitOfWork.CarCarColors.GetByIdAsync(carId, colorId);
            if (existing == null)
            {
                throw new Exception("CarCarColor not found");
            }

            if (dto.IsAvailable == null)
            {
                dto.IsAvailable = existing.IsAvailable;
            }

            // Preserve existing values if caller omitted them.
            if (dto.StockQuantity == null)
            {
                dto.StockQuantity = existing.StockQuantity;
            }

            if (dto.PricingPerColor == null)
            {
                dto.PricingPerColor = existing.PricingPerColor;
            }

            if (string.IsNullOrWhiteSpace(dto.ColorImageUrl))
            {
                dto.ColorImageUrl = existing.ColorImageUrl;
            }

            _mapper.Map(dto, existing);
            await _unitOfWork.CarCarColors.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int carId, int colorId)
        {
            var existing = await _unitOfWork.CarCarColors.GetByIdAsync(carId, colorId);
            if (existing == null)
            {
                throw new Exception("CarCarColor not found");
            }

            await _unitOfWork.CarCarColors.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
