using AutoMapper;
using CarGalary.Application.Dtos.Car.Command;
using CarGalary.Application.Dtos.Car.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CarService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<CarResponseDto>> GetAllAsync()
        {
            var cars = await _unitOfWork.Cars.GetAllAsync();
            return _mapper.Map<List<CarResponseDto>>(cars);
        }

        public async Task<CarResponseDto?> GetByIdAsync(int id)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(id);
            return car == null ? null : _mapper.Map<CarResponseDto>(car);
        }

        public async Task<CarResponseDto> CreateAsync(CreateCarRequestDto dto)
        {
            var model = await _unitOfWork.CarModels.GetByIdAsync(dto.ModelId);
            if (model == null)
            {
                throw new Exception("CarModel not found");
            }

            var type = await _unitOfWork.CarTypes.GetCarTypeById(dto.TypeId);
            if (type == null)
            {
                throw new Exception("CarType not found");
            }

            var entity = _mapper.Map<Car>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.Cars.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateCarRequestDto dto)
        {
            var existing = await _unitOfWork.Cars.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Car not found");
            }

            var model = await _unitOfWork.CarModels.GetByIdAsync(dto.ModelId);
            if (model == null)
            {
                throw new Exception("CarModel not found");
            }

            var type = await _unitOfWork.CarTypes.GetCarTypeById(dto.TypeId);
            if (type == null)
            {
                throw new Exception("CarType not found");
            }

            if (dto.IsAvailable == null)
            {
                dto.IsAvailable = existing.IsAvailable;
            }

            _mapper.Map(dto, existing);
            await _unitOfWork.Cars.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.Cars.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Car not found");
            }

            await _unitOfWork.Cars.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<CarResponseDto>> FilterAsync(int? modelId = null, int? typeId = null, bool? isAvailable = null)
        {
            var cars = await _unitOfWork.Cars.FilterAsync(modelId, typeId, isAvailable);
            return _mapper.Map<List<CarResponseDto>>(cars);
        }
    }
}
