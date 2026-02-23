using AutoMapper;
using CarGalary.Application.Dtos.CarColor.Command;
using CarGalary.Application.Dtos.CarColor.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class CarColorService : ICarColorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CarColorService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<CarColorResponseDto>> GetAllAsync()
        {
            var colors = await _unitOfWork.CarColors.GetAllCarColorsAsync();
            var available = colors.Where(x => x.IsAvailable).ToList();
            return _mapper.Map<List<CarColorResponseDto>>(available);
        }

        public async Task<CarColorResponseDto?> GetByIdAsync(int id)
        {
            var color = await _unitOfWork.CarColors.GetCarColorByIdAsync(id);
            if (color == null || !color.IsAvailable) return null;
            return _mapper.Map<CarColorResponseDto>(color);
        }

        public async Task<CarColorResponseDto> CreateAsync(CreateCarColorRequestDto dto)
        {
            var entity = _mapper.Map<Color>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.CarColors.AddCarColorAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarColorResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateCarColorRequestDto dto)
        {
            var existing = await _unitOfWork.CarColors.GetCarColorByIdAsync(id);
            if (existing == null || !existing.IsAvailable)
            {
                throw new Exception("CarColor not found");
            }

            if (dto.IsAvailable == null)
            {
                dto.IsAvailable = existing.IsAvailable;
            }

            _mapper.Map(dto, existing);
            await _unitOfWork.CarColors.UpdateCarColorAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.CarColors.GetCarColorByIdAsync(id);
            if (existing == null || !existing.IsAvailable)
            {
                throw new Exception("CarColor not found");
            }

            await _unitOfWork.CarColors.DeleteCarColorAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

