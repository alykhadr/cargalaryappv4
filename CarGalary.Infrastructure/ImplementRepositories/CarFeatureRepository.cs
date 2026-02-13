
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
   public class CarFeatureRepository : ICarFeatureRepository
{
    private readonly ApplicationDbContext _context;

    public CarFeatureRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<List<CarFeature>> GetAllAsync()
    {
        return await _context.CarFeatures
           
            .ToListAsync();
    }

    public async Task CreateAsync(CarFeature carFeature)
    {
        
        _context.CarFeatures.Add(carFeature);
    }

    public async Task AssignFeaturesToCarAsync(
        int carId, List<int> featureIds)
    {
        

        

        // Remove old features
        var existing = _context.CarCarFeatures
            .Where(x => x.CarId == carId);

        _context.CarCarFeatures.RemoveRange(existing);

        // Add new features
        foreach (var featureId in featureIds.Distinct())
        {
            _context.CarCarFeatures.Add(new CarCarFeature
            {
                CarId = carId,
                CarFeatureId = featureId
            });
        }

        
    }
}
}