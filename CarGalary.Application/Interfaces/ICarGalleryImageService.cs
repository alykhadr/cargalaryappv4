
using CarGalary.Application.Dtos.CarGalleryImage.Command;
using CarGalary.Application.Dtos.CarGalleryImage.Query;

namespace CarGalary.Application.Interfaces
{
    public interface ICarGalleryImageService
    {
        Task<List<CarGalleryImageResponseDto>> GetAllAsync();
        Task<CarGalleryImageResponseDto?> GetByIdAsync(int id);
        Task<List<CarGalleryImageResponseDto>> GetByCarIdAsync(int carId);
        Task<CarGalleryImageResponseDto> CreateAsync(CreateCarGalleryImageRequestDto dto);
        Task UpdateAsync(int id, UpdateCarGalleryImageRequestDto dto);
        Task DeleteAsync(int id);
    }
}
