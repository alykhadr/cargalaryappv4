using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IInvoiceRepository
    {
        Task<List<Invoice>> GetAllAsync();
        Task<List<Invoice>> GetAllByBranchAsync(int branchId);
        Task<Invoice?> GetByIdAsync(int id);
        Task<Invoice?> GetByIdAsync(int id, int branchId);
        Task<Invoice?> GetByIdForUpdateAsync(int id);
        Task<Invoice?> GetByIdForUpdateAsync(int id, int branchId);
        Task CreateAsync(Invoice invoice);
        Task<bool> UserExistsAsync(Guid userId);
        Task<bool> InvoiceNumberExistsAsync(string invoiceNumber, int? excludeInvoiceId = null);
    }
}
