using AutoMapper;
using CarGalary.Application.Dtos.EngineSpecification.Command;
using CarGalary.Application.Dtos.EngineSpecification.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Services
{
    public class EngineSpecificationService : IEngineSpecificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public EngineSpecificationService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<EngineSpecificationResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.EngineSpecifications.GetAllAsync();
            return _mapper.Map<List<EngineSpecificationResponseDto>>(items);
        }

        public async Task<EngineSpecificationResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.EngineSpecifications.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<EngineSpecificationResponseDto>(item);
        }

        public async Task<EngineSpecificationResponseDto> CreateAsync(CreateEngineSpecificationRequestDto dto)
        {
            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            var entity = _mapper.Map<EngineSpecification>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.EngineSpecifications.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<EngineSpecificationResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateEngineSpecificationRequestDto dto)
        {
            var existing = await _unitOfWork.EngineSpecifications.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("EngineSpecification not found");
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
            await _unitOfWork.EngineSpecifications.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.EngineSpecifications.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("EngineSpecification not found");
            }

            await _unitOfWork.EngineSpecifications.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
