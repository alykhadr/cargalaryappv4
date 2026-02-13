

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ICarTypeRepository
    {
         Task<List<CarType>> GetCarTypes();
          Task<CarType> GetCarTypeById(int id);

          Task CreateCarType(CarType type);

        Task UpdateCarType(CarType type);
         Task<CarType>  CarTypeExists(int id);

        Task DeleteCarTypeById(CarType carType);
    }
}