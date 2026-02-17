using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ISeatingRepository
    {
        Task<IEnumerable<Seating>> GetAllAsync();
        Task<Seating> GetByIdAsync(int id);
        Task CreateAsync(Seating model);
        Task UpdateAsync(Seating model);
        Task DeleteAsync(Seating model);
    }
}
