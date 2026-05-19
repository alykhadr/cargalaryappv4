using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class PrivacyPolicyRepository : IPrivacyPolicyRepository
    {
        private readonly ApplicationDbContext _context;

        public PrivacyPolicyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PrivacyPolicy>> GetAllAsync()
        {
            return await _context.PrivacyPolicies
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<PrivacyPolicy?> GetFirstAsync()
        {
            return await _context.PrivacyPolicies
                .Where(x => x.IsAvailable)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<PrivacyPolicy?> GetByIdAsync(int id)
        {
            return await _context.PrivacyPolicies.FindAsync(id);
        }

        public Task CreateAsync(PrivacyPolicy privacyPolicy)
        {
            _context.PrivacyPolicies.Add(privacyPolicy);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(PrivacyPolicy privacyPolicy)
        {
            _context.Entry(privacyPolicy).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(PrivacyPolicy privacyPolicy)
        {
            _context.PrivacyPolicies.Remove(privacyPolicy);
            return Task.CompletedTask;
        }
    }
}
