using AutoMapper;
using CarGalary.Application.Dtos.Transmission.Command;
using CarGalary.Application.Dtos.Transmission.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Services
{
    public class TransmissionService : ITransmissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public TransmissionService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<TransmissionResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.Transmissions.GetAllAsync();
            return _mapper.Map<List<TransmissionResponseDto>>(items);
        }

        public async Task<TransmissionResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.Transmissions.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<TransmissionResponseDto>(item);
        }

        public async Task<TransmissionResponseDto> CreateAsync(CreateTransmissionRequestDto dto)
        {
            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            var entity = _mapper.Map<Transmission>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.Transmissions.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TransmissionResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateTransmissionRequestDto dto)
        {
            var existing = await _unitOfWork.Transmissions.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Transmission not found");
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
            await _unitOfWork.Transmissions.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.Transmissions.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Transmission not found");
            }

            await _unitOfWork.Transmissions.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
