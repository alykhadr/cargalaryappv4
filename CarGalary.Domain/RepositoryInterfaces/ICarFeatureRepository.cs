




namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ICarFeatureRepository
    {
        Task<List<CarFeature>> GetAllAsync();
        Task<CarFeature> GetByIdAsync(int id);
        Task CreateAsync(CarFeature carFeature);
        Task UpdateAsync(CarFeature carFeature);
        Task DeleteAsync(CarFeature carFeature);
        Task AssignFeaturesToCarAsync(int carId, List<int> featureIds);
    }
}
