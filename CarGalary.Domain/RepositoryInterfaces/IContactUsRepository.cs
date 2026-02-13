

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IContactUsRepository
    {
        Task<IEnumerable<ContactUs>> GetAllAsync();
    }
}