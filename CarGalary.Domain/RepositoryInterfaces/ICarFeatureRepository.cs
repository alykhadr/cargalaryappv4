




namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ICarFeatureRepository
    {
        //TODO
        Task<List<CarFeature>> GetAllAsync();
        Task CreateAsync(CarFeature carFeature);
        Task AssignFeaturesToCarAsync(int carId, List<int> featureIds);
    }
}