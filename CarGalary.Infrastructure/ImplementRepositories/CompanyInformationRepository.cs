
using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
     public class CompanyInformationRepository : ICompanyInformationRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyInformationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CompanyInformation company)
        {
            _context.CompanyInformations.Add(company);

        }

        public async Task DeleteAsync(CompanyInformation companyInformation)
        {
            
            _context.CompanyInformations.Remove(companyInformation);

        }

        public async Task<IEnumerable<CompanyInformation>> GetAllAsync()
        {
            return await _context.CompanyInformations.ToListAsync();
        }

        public async Task<CompanyInformation> GetByIdAsync(int id)
        {
            return await _context.CompanyInformations.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(CompanyInformation company)
        {
            

           _context.Entry(company).State = EntityState.Modified;
        }
    }
}