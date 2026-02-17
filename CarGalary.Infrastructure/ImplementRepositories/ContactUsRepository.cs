
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class ContactUsRepository : IContactUsRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactUsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContactUs>> GetAllAsync()
        {
            return await _context.ContactUs
                                 .OrderByDescending(c => c.CreatedAt)
                                 .ToListAsync();
        }

        public async Task<ContactUs> GetByIdAsync(int id)
        {
            return await _context.ContactUs.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateAsync(ContactUs contactUs)
        {
            _context.ContactUs.Add(contactUs);
        }

        public async Task UpdateAsync(ContactUs contactUs)
        {
            _context.Entry(contactUs).State = EntityState.Modified;
        }

        public async Task DeleteAsync(ContactUs contactUs)
        {
            _context.ContactUs.Remove(contactUs);
        }
    }
}
