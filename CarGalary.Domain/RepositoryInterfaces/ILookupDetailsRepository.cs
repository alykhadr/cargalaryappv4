using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ILookupDetailsRepository
    {
        Task<List<LookupDetails>> GetByMasterCodeAsync(string masterCode);
        Task<LookupDetails?> GetByMasterAndDetailAsync(string masterCode, string detailCode);
    }
}
