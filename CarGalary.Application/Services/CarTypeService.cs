using AutoMapper;
using CarGalary.Application.Dtos.CarType.Command;
using CarGalary.Application.Dtos.CarType.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class CarTypeService : ICarTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CarTypeService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<CarTypeResponseDto>> GetAllAsync()
        {
            var carTypes = await _unitOfWork.CarTypes.GetCarTypes();
            return _mapper.Map<List<CarTypeResponseDto>>(carTypes);
        }

        public async Task<CarTypeResponseDto?> GetByIdAsync(int id)
        {
            var carType = await _unitOfWork.CarTypes.GetCarTypeById(id);
            return carType == null ? null : _mapper.Map<CarTypeResponseDto>(carType);
        }

        public async Task<CarTypeResponseDto> CreateAsync(CreateCarTypeRequestDto dto)
        {
            var entity = _mapper.Map<CarType>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.CarTypes.CreateCarType(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarTypeResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateCarTypeRequestDto dto)
        {
            var existing = await _unitOfWork.CarTypes.GetCarTypeById(id);
            if (existing == null)
            {
                throw new Exception("CarType not found");
            }

            if (dto.IsAvailable == null)
            {
                dto.IsAvailable = existing.IsAvailable;
            }

            _mapper.Map(dto, existing);
            await _unitOfWork.CarTypes.UpdateCarType(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.CarTypes.GetCarTypeById(id);
            if (existing == null)
            {
                throw new Exception("CarType not found");
            }

            await _unitOfWork.CarTypes.DeleteCarTypeById(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
