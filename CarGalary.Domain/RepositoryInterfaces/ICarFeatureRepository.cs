




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
        Task AddCarFeatureAssignmentAsync(CarFeature carFeature);
        Task<List<CarFeature>> GetCarFeatureAssignmentsByCarIdAsync(int carId);
        Task<CarFeature?> GetCarFeatureAssignmentAsync(int carId, int featureId);
        Task UpdateCarFeatureAssignmentAsync(CarFeature carFeature);
        Task DeleteCarFeatureAssignmentAsync(CarFeature carFeature);
    }
}
