
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


    public async Task<List<Feature>> GetAllAsync()
    {
        return await _context.Features
           
            .ToListAsync();
    }
    
    public async Task<Feature> GetByIdAsync(int id)
    {
        return await _context.Features.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateAsync(Feature carFeature)
    {
        
        _context.Features.Add(carFeature);
    }

    public async Task UpdateAsync(Feature carFeature)
    {
        _context.Entry(carFeature).State = EntityState.Modified;
    }

    public async Task DeleteAsync(Feature carFeature)
    {
        _context.Features.Remove(carFeature);
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
                FeatureId = featureId
            });
        }

        
    }
}
}
