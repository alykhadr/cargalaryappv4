using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IQuotationHistoryRepository
    {
        Task CreateAsync(QuotationHistory quotationHistory);
        Task<List<QuotationHistory>> GetByQuotationIdAsync(int quotationId);
        Task<bool> ExistsByQuotationAndStatusAsync(int quotationId, int status);
    }
}
