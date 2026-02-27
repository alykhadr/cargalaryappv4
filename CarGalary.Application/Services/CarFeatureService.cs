using AutoMapper;
using CarGalary.Application.Dtos.CarFeature.Command;
using CarGalary.Application.Dtos.CarFeature.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class CarFeatureService : ICarFeatureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CarFeatureService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<CarFeatureResponseDto>> GetAllAsync()
        {
            var features = await _unitOfWork.CarFeatures.GetAllAsync();
            return _mapper.Map<List<CarFeatureResponseDto>>(features);
        }

        public async Task<CarFeatureResponseDto?> GetByIdAsync(int id)
        {
            var feature = await _unitOfWork.CarFeatures.GetByIdAsync(id);
            return feature == null ? null : _mapper.Map<CarFeatureResponseDto>(feature);
        }

        public async Task<CarFeatureResponseDto> CreateAsync(CreateCarFeatureRequestDto dto)
        {
            var entity = _mapper.Map<Feature>(dto);
            entity.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.CarFeatures.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarFeatureResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateCarFeatureRequestDto dto)
        {
            var existing = await _unitOfWork.CarFeatures.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("CarFeature not found");
            }

            if (dto.IsAvailable == null)
            {
                dto.IsAvailable = existing.IsAvailable;
            }

            _mapper.Map(dto, existing);
            await _unitOfWork.CarFeatures.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.CarFeatures.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("CarFeature not found");
            }

            await _unitOfWork.CarFeatures.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<CarFeatureAssignmentResponseDto>> GetAssignmentsByCarIdAsync(int carId)
        {
            var items = await _unitOfWork.CarFeatures.GetCarFeatureAssignmentsByCarIdAsync(carId);
            return items.Select(x => new CarFeatureAssignmentResponseDto
            {
                CarId = x.CarId,
                FeatureId = x.FeatureId,
                IsAvailable = x.IsAvailable,
                CreatedBy = x.CreatedBy,
                CreatedAt = x.CreatedAt
            }).ToList();
        }

        public async Task<CarFeatureAssignmentResponseDto> CreateAssignmentAsync(int carId, CreateCarFeatureAssignmentRequestDto dto)
        {
            if (dto == null || dto.FeatureId <= 0)
            {
                throw new Exception("FeatureId is required");
            }

            var carExists = await _unitOfWork.Cars.GetByIdAsync(carId);
            if (carExists == null)
            {
                throw new Exception("CarId is not valid");
            }

            var featureExists = await _unitOfWork.CarFeatures.GetByIdAsync(dto.FeatureId);
            if (featureExists == null)
            {
                throw new Exception("FeatureId is not valid");
            }

            var existing = await _unitOfWork.CarFeatures.GetCarFeatureAssignmentAsync(carId, dto.FeatureId);
            if (existing != null)
            {
                throw new Exception("Feature already assigned to this car");
            }

            var entity = new CarFeature
            {
                CarId = carId,
                FeatureId = dto.FeatureId,
                IsAvailable = dto.IsAvailable,
                CreatedBy = _currentUserService.UserName,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.CarFeatures.AddCarFeatureAssignmentAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return new CarFeatureAssignmentResponseDto
            {
                CarId = entity.CarId,
                FeatureId = entity.FeatureId,
                IsAvailable = entity.IsAvailable,
                CreatedBy = entity.CreatedBy,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task UpdateAssignmentAsync(int carId, int featureId, UpdateCarFeatureAssignmentRequestDto dto)
        {
            var existing = await _unitOfWork.CarFeatures.GetCarFeatureAssignmentAsync(carId, featureId);
            if (existing == null)
            {
                throw new Exception("Car feature assignment not found");
            }

            existing.IsAvailable = dto.IsAvailable;
            await _unitOfWork.CarFeatures.UpdateCarFeatureAssignmentAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAssignmentAsync(int carId, int featureId)
        {
            var existing = await _unitOfWork.CarFeatures.GetCarFeatureAssignmentAsync(carId, featureId);
            if (existing == null)
            {
                throw new Exception("Car feature assignment not found");
            }

            await _unitOfWork.CarFeatures.DeleteCarFeatureAssignmentAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
