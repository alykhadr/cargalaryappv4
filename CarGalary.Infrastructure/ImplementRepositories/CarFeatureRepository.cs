
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
        var existing = _context.CarFeatures
            .Where(x => x.CarId == carId);

        _context.CarFeatures.RemoveRange(existing);

        // Add new features
        foreach (var featureId in featureIds.Distinct())
        {
            _context.CarFeatures.Add(new CarFeature
            {
                CarId = carId,
                FeatureId = featureId
            });
        }

        
    }

    public Task AddCarFeatureAssignmentAsync(CarFeature carFeature)
    {
        _context.CarFeatures.Add(carFeature);
        return Task.CompletedTask;
    }

    public async Task<List<CarFeature>> GetCarFeatureAssignmentsByCarIdAsync(int carId)
    {
        return await _context.CarFeatures
            .AsNoTracking()
            .Where(x => x.CarId == carId)
            .OrderBy(x => x.FeatureId)
            .ToListAsync();
    }

    public async Task<CarFeature?> GetCarFeatureAssignmentAsync(int carId, int featureId)
    {
        return await _context.CarFeatures.FindAsync(carId, featureId);
    }

    public Task UpdateCarFeatureAssignmentAsync(CarFeature carFeature)
    {
        _context.Entry(carFeature).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task DeleteCarFeatureAssignmentAsync(CarFeature carFeature)
    {
        _context.CarFeatures.Remove(carFeature);
        return Task.CompletedTask;
    }
}
}
