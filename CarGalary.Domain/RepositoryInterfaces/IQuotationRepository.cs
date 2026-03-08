using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IQuotationRepository
    {
        Task<List<Quotation>> GetAllAsync();
        Task<List<Quotation>> GetAllByBranchAsync(int branchId);
        Task CreateAsync(Quotation quotation);
        Task<Quotation?> GetByIdAsync(int id);
        Task<Quotation?> GetByIdAsync(int id, int branchId);
        Task<Quotation?> GetByIdForUpdateAsync(int id);
        Task<Quotation?> GetByIdForUpdateAsync(int id, int branchId);
        Task<bool> UserHasQuotationAsync(Guid userId);
        Task<bool> UserExistsAsync(Guid userId);
    }
}
