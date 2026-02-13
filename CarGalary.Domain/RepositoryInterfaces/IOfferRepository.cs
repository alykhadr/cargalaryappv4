

using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IOfferRepository
    {
         Task<IEnumerable<Offer>> GetAllAsync();
        Task<Offer> GetByIdAsync(int id);
        Task CreateAsync(Offer offer);
        Task UpdateAsync(Offer offer);
        Task DeleteAsync(Offer offer);
    }
}