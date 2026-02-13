

using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{


    public class MemberServiceRepository : IMemberServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public MemberServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MemberService>> GetAllAsync()
        {
            return await _context.MemberServices.ToListAsync();
        }

        public async Task<MemberService> GetByIdAsync(int id)
        {
            return await _context.MemberServices.FindAsync(id);
        }

        public async Task CreateAsync(MemberService memberService)
        {
            _context.MemberServices.Add(memberService);
        }

        public async Task UpdateAsync(MemberService memberService)
        {
            _context.Entry(memberService).State = EntityState.Modified;


        }

        public async Task DeleteAsync(MemberService memberService)
        {

            _context.MemberServices.Remove(memberService);


        }
    }

}