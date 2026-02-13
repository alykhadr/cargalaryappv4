

using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ICompanyInformationRepository
    {
        
        Task<IEnumerable<CompanyInformation>> GetAllAsync();
        Task<CompanyInformation> GetByIdAsync(int id);
        Task CreateAsync(CompanyInformation company);
        Task UpdateAsync(CompanyInformation company);
        Task DeleteAsync(CompanyInformation   companyInformation);
    }
}