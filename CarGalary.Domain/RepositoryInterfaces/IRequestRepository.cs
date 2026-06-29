using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IRequestRepository
    {
        Task<List<Request>> GetAllAsync();
        Task<List<Request>> GetAllByBranchAsync(int branchId);
        Task CreateAsync(Request request);
        Task<Request?> GetByIdAsync(int id);
        Task<Request?> GetByIdAsync(int id, int branchId);
        Task<Request?> GetByIdForUpdateAsync(int id);
        Task<Request?> GetByIdForUpdateAsync(int id, int branchId);
        Task<bool> UserExistsAsync(Guid userId);
    }
}
