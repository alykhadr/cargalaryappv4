using AutoMapper;
using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Command;
using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class AudioAndCommunicationSystemService : IAudioAndCommunicationSystemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public AudioAndCommunicationSystemService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<AudioAndCommunicationSystemResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.AudioAndCommunicationSystems.GetAllAsync();
            return _mapper.Map<List<AudioAndCommunicationSystemResponseDto>>(items);
        }

        public async Task<AudioAndCommunicationSystemResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.AudioAndCommunicationSystems.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<AudioAndCommunicationSystemResponseDto>(item);
        }

        public async Task<List<AudioAndCommunicationSystemResponseDto>> GetByCarIdAsync(int carId)
        {
            var items = await _unitOfWork.AudioAndCommunicationSystems.GetByCarIdAsync(carId);
            return _mapper.Map<List<AudioAndCommunicationSystemResponseDto>>(items);
        }

        public async Task<AudioAndCommunicationSystemResponseDto> CreateAsync(CreateAudioAndCommunicationSystemRequestDto dto)
        {
            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            var entity = _mapper.Map<AudioAndCommunicationSystem>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.AudioAndCommunicationSystems.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AudioAndCommunicationSystemResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateAudioAndCommunicationSystemRequestDto dto)
        {
            var existing = await _unitOfWork.AudioAndCommunicationSystems.GetByIdAsync(id);
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
            await _unitOfWork.AudioAndCommunicationSystems.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.AudioAndCommunicationSystems.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("AudioAndCommunicationSystem not found");
            }

            await _unitOfWork.AudioAndCommunicationSystems.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

