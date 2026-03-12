using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IRequestHistoryRepository
    {
        Task CreateAsync(RequestHistory requestHistory);
        Task<List<RequestHistory>> GetByRequestIdAsync(int requestId);
        Task<bool> ExistsByRequestAndStatusAsync(int requestId, int status);
    }
}
