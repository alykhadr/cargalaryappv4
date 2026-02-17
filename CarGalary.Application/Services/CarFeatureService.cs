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

        public CarFeatureService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            var entity = _mapper.Map<CarFeature>(dto);
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
    }
}
