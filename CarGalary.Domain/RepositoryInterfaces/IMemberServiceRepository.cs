


using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IMemberServiceRepository
{
    Task<IEnumerable<MemberService>> GetAllAsync();
    Task<MemberService> GetByIdAsync(int id);
    Task CreateAsync(MemberService memberService);
    Task UpdateAsync(MemberService memberService);
    Task DeleteAsync(MemberService memberService);
}

}