
using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly ApplicationDbContext _context;

        public OfferRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Offer offer)
        {
            _context.Offers.Add(offer);
        }

        public async Task DeleteAsync(Offer offer)
        {
           
            _context.Offers.Remove(offer);

        }

        public async Task<IEnumerable<Offer>> GetAllAsync()
        {
            return await _context.Offers
                                 .OrderByDescending(o => o.CreatedAt)
                                 .ToListAsync();
        }

        public async Task<Offer> GetByIdAsync(int id)
        {
            return await _context.Offers.FindAsync(id);
        }

        public async Task UpdateAsync(Offer offer)
        {
             _context.Entry(offer).State = EntityState.Modified;


            
        }
    }
}