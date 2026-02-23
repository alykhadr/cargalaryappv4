using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarGalary.Domain.Entities;


namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ICarColorRepository
{
     Task AddCarColorAsync(Color carColor);
    Task<Color> GetCarColorByIdAsync(int id);
    Task<IEnumerable<Color>> GetAllCarColorsAsync();
    Task UpdateCarColorAsync(Color carColor);
    Task DeleteCarColorAsync(Color carColor);
}
}