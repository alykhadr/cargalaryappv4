using AutoMapper;
using CarGalary.Application.Dtos.CarGalleryImage.Command;
using CarGalary.Application.Dtos.CarGalleryImage.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class CarGalleryImageService : ICarGalleryImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CarGalleryImageService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<CarGalleryImageResponseDto>> GetAllAsync()
        {
            var images = await _unitOfWork.CarGalleryImages.GetAllImagesAsync();
            return _mapper.Map<List<CarGalleryImageResponseDto>>(images);
        }

        public async Task<CarGalleryImageResponseDto?> GetByIdAsync(int id)
        {
            var image = await _unitOfWork.CarGalleryImages.GetImageByIdAsync(id);
            return image == null ? null : _mapper.Map<CarGalleryImageResponseDto>(image);
        }

        public async Task<List<CarGalleryImageResponseDto>> GetByCarIdAsync(int carId)
        {
            var images = await _unitOfWork.CarGalleryImages.GetImagesByCarAsync(carId);
            return _mapper.Map<List<CarGalleryImageResponseDto>>(images);
        }

        public async Task<CarGalleryImageResponseDto> CreateAsync(CreateCarGalleryImageRequestDto dto)
        {
            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            var image = _mapper.Map<CarGalleryImage>(dto);
            image.CreatedAt = DateTime.UtcNow;
            image.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.CarGalleryImages.AddImageAsync(image);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarGalleryImageResponseDto>(image);
        }

        public async Task UpdateAsync(int id, UpdateCarGalleryImageRequestDto dto)
        {
            var existing = await _unitOfWork.CarGalleryImages.GetImageByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("CarGalleryImage not found");
            }

            var car = await _unitOfWork.Cars.CarExistsAsync(dto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }

            if (string.IsNullOrWhiteSpace(dto.ImageUrl))
            {
                dto.ImageUrl = existing.ImageUrl;
            }

            _mapper.Map(dto, existing);
            await _unitOfWork.CarGalleryImages.UpdateImageAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.CarGalleryImages.GetImageByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("CarGalleryImage not found");
            }

            await _unitOfWork.CarGalleryImages.DeleteImageAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
