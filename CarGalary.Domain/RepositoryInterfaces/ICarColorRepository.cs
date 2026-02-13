using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarGalary.Domain.Entities;


namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ICarColorRepository
{
     Task AddCarColorAsync(CarColor carColor);
    Task<CarColor> GetCarColorByIdAsync(int id);
    Task<IEnumerable<CarColor>> GetAllCarColorsAsync();
    Task UpdateCarColorAsync(CarColor carColor);
    Task DeleteCarColorAsync(CarColor carColor);
}
}