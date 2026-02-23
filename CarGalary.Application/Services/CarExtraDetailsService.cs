using AutoMapper;
using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Command;
using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class CarExtraDetailsService : ICarExtraDetailsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CarExtraDetailsService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<CarExtraDetailsResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.CarExtraDetails.GetAllAsync();
            return _mapper.Map<List<CarExtraDetailsResponseDto>>(items);
        }

        public async Task<CarExtraDetailsResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.CarExtraDetails.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<CarExtraDetailsResponseDto>(item);
        }

        public async Task<List<CarExtraDetailsResponseDto>> GetByCarIdAsync(int carId)
        {
            var items = await _unitOfWork.CarExtraDetails.GetByCarIdAsync(carId);
            return _mapper.Map<List<CarExtraDetailsResponseDto>>(items);
        }

        public async Task<CarExtraDetailsResponseDto> CreateAsync(CreateCarExtraDetailsRequestDto dto)
        {
            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            var entity = _mapper.Map<CarExtraDetails>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.CarExtraDetails.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarExtraDetailsResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateCarExtraDetailsRequestDto dto)
        {
            var existing = await _unitOfWork.CarExtraDetails.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("AudioAndCommunicationSystem not found");
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
            await _unitOfWork.CarExtraDetails.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.CarExtraDetails.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("AudioAndCommunicationSystem not found");
            }

            await _unitOfWork.CarExtraDetails.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

