
using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ICarGalleryImageRepository
    {
        Task AddImageAsync(CarGalleryImage image);
        Task<CarGalleryImage> GetImageByIdAsync(int imageId);
        Task<IEnumerable<CarGalleryImage>> GetImagesByCarAsync(int carId);
        Task UpdateImageAsync(CarGalleryImage image);
        Task DeleteImageAsync(CarGalleryImage image);
    }
}