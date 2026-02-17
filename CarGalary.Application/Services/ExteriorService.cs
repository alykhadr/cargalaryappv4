using AutoMapper;
using CarGalary.Application.Dtos.Exterior.Command;
using CarGalary.Application.Dtos.Exterior.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Services
{
    public class ExteriorService : IExteriorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public ExteriorService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<ExteriorResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.Exteriors.GetAllAsync();
            return _mapper.Map<List<ExteriorResponseDto>>(items);
        }

        public async Task<ExteriorResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.Exteriors.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<ExteriorResponseDto>(item);
        }

        public async Task<ExteriorResponseDto> CreateAsync(CreateExteriorRequestDto dto)
        {
            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            var entity = _mapper.Map<Exterior>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.Exteriors.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ExteriorResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateExteriorRequestDto dto)
        {
            var existing = await _unitOfWork.Exteriors.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Exterior not found");
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
            await _unitOfWork.Exteriors.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.Exteriors.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Exterior not found");
            }

            await _unitOfWork.Exteriors.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
