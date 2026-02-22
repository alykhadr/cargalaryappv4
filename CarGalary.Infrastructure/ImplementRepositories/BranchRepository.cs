

using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly ApplicationDbContext _context;

        public BranchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Branchs> CreateAsync(Branchs branch)
        {
           await _context.Branches.AddAsync(branch);
           
            return branch;
        }

        public async Task DeleteAsync(Branchs branch)
        {

            _context.Branches.Remove(branch);
           
        }

        public async Task<IEnumerable<Branchs>> GetAllAsync()
        {
            return await _context.Branches
                                           .Include(c=>c.BranchWorkingDays)
                                           .ToListAsync();
        }

        public async Task<Branchs> GetByIdAsync(int id)
        {
            return await _context.Branches.Include(c=>c.BranchWorkingDays).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateAsync(Branchs branch)
        {
            _context.Entry(branch).State = EntityState.Modified;
           
        }

        // public async Task<IEnumerable<BranchWorkingDays>> GetWorkingDaysAsync(int branchId)
        // {
        //     return await _context.BranchWorkingDays
        //                          .Where(w => w.BranchId == branchId)
        //                          .ToListAsync();
        // }

        public async Task AddWorkingDayAsync(BranchWorkingDays workingDay)
        {
            _context.BranchWorkingDays.Add(workingDay);
        }

        public async Task UpdateWorkingDayAsync(BranchWorkingDays workingDay)
        {
             _context.Entry(workingDay).State = EntityState.Modified;
          
        }

        public async Task DeleteWorkingDayAsync(BranchWorkingDays workingDay)
        {
            
            _context.BranchWorkingDays.Remove(workingDay);
            
        }

        public  async Task DeActiveAsync(Branchs branch)
        {
            _context.Entry(branch).State = EntityState.Modified;
        }

        public async Task ActiveAsync(Branchs branch)
        {
            _context.Entry(branch).State = EntityState.Modified;
            foreach (var wd in branch.BranchWorkingDays)
            {
                _context.Entry(wd).State = EntityState.Modified;
            }
        }
    }
}