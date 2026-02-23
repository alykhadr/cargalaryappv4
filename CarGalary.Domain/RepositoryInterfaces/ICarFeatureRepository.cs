




namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ICarFeatureRepository
    {
        Task<List<Feature>> GetAllAsync();
        Task<Feature> GetByIdAsync(int id);
        Task CreateAsync(Feature carFeature);
        Task UpdateAsync(Feature carFeature);
        Task DeleteAsync(Feature carFeature);
        Task AssignFeaturesToCarAsync(int carId, List<int> featureIds);
    }
}
