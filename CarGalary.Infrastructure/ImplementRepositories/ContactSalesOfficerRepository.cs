
using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class ContactSalesOfficerRepository : IContactSalesOfficerRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactSalesOfficerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContactSalesOfficer>> GetAllAsync()
        {
            return await _context.ContactSalesOfficers.ToListAsync();
        }

        public async Task<ContactSalesOfficer> GetByIdAsync(int id)
        {
            return await _context.ContactSalesOfficers.FindAsync(id);
        }

        public async Task CreateAsync(ContactSalesOfficer  contactSalesOfficer)
        {
            _context.ContactSalesOfficers.Add(contactSalesOfficer);
        }

        public async Task UpdateAsync(ContactSalesOfficer contactSalesOfficer)
        {

            _context.Entry(contactSalesOfficer).State = EntityState.Modified;

        }

        public async Task DeleteAsync(ContactSalesOfficer contactSalesOfficer)
        {

            _context.ContactSalesOfficers.Remove(contactSalesOfficer);

        }
    }
}