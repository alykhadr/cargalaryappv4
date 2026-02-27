using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IQuotationRepository
    {
        Task<List<Quotation>> GetAllAsync();
        Task CreateAsync(Quotation quotation);
        Task<Quotation?> GetByIdAsync(int id);
        Task<bool> UserHasQuotationAsync(Guid userId);
        Task<bool> UserExistsAsync(Guid userId);
    }
}
