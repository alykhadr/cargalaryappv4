

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IContactUsRepository
    {
        Task<IEnumerable<ContactUs>> GetAllAsync();
        Task<ContactUs> GetByIdAsync(int id);
        Task CreateAsync(ContactUs contactUs);
        Task UpdateAsync(ContactUs contactUs);
        Task DeleteAsync(ContactUs contactUs);
    }
}
