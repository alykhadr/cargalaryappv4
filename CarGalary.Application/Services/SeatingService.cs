using AutoMapper;
using CarGalary.Application.Dtos.Seating.Command;
using CarGalary.Application.Dtos.Seating.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Services
{
    public class SeatingService : ISeatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public SeatingService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<SeatingResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.Seatings.GetAllAsync();
            return _mapper.Map<List<SeatingResponseDto>>(items);
        }

        public async Task<SeatingResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.Seatings.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<SeatingResponseDto>(item);
        }

        public async Task<SeatingResponseDto> CreateAsync(CreateSeatingRequestDto dto)
        {
            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            var entity = _mapper.Map<Seating>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.Seatings.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SeatingResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateSeatingRequestDto dto)
        {
            var existing = await _unitOfWork.Seatings.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Seating not found");
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
            await _unitOfWork.Seatings.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.Seatings.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Seating not found");
            }

            await _unitOfWork.Seatings.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
