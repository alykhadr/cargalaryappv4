using AutoMapper;
using CarGalary.Application.Dtos.ExtraFeature.Command;
using CarGalary.Application.Dtos.ExtraFeature.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Services
{
    public class ExtraFeatureService : IExtraFeatureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public ExtraFeatureService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<ExtraFeatureResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.ExtraFeatures.GetAllAsync();
            return _mapper.Map<List<ExtraFeatureResponseDto>>(items);
        }

        public async Task<ExtraFeatureResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.ExtraFeatures.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<ExtraFeatureResponseDto>(item);
        }

        public async Task<ExtraFeatureResponseDto> CreateAsync(CreateExtraFeatureRequestDto dto)
        {
            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            var entity = _mapper.Map<ExtraFeature>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.ExtraFeatures.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ExtraFeatureResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateExtraFeatureRequestDto dto)
        {
            var existing = await _unitOfWork.ExtraFeatures.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("ExtraFeature not found");
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
            await _unitOfWork.ExtraFeatures.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.ExtraFeatures.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("ExtraFeature not found");
            }

            await _unitOfWork.ExtraFeatures.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
