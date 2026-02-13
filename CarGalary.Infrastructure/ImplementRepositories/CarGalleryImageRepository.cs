
using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class CarGalleryImageRepository : ICarGalleryImageRepository
{
    private readonly ApplicationDbContext _context;

    public CarGalleryImageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddImageAsync(CarGalleryImage image)
    {
        _context.CarGalleryImages.Add(image);
    }

    public async Task<CarGalleryImage> GetImageByIdAsync(int imageId)
    {
        return await _context.CarGalleryImages.FindAsync(imageId);
    }

    public async Task<IEnumerable<CarGalleryImage>> GetImagesByCarAsync(int carId)
    {
        return await _context.CarGalleryImages
            .Where(i => i.CarId == carId)
            .ToListAsync();
    }

    public async Task UpdateImageAsync(CarGalleryImage image)
    {
        
_context.Entry(image).State = EntityState.Modified;
    
    }

    public async Task DeleteImageAsync(CarGalleryImage carGalleryImage)
    {
        _context.CarGalleryImages.Remove(carGalleryImage);
      
    }
}
}