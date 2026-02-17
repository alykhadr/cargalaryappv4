
using CarGalary.Application.Dtos.CompanyInformation.Command;
using CarGalary.Application.Dtos.CompanyInformation.Query;

namespace CarGalary.Application.Interfaces
{
    public interface ICompanyInformationService
    {
        Task<List<CompanyInformationResponseDto>> GetAllAsync();
        Task<CompanyInformationResponseDto?> GetByIdAsync(int id);
        Task<CompanyInformationResponseDto> CreateAsync(CreateCompanyInformationRequestDto dto);
        Task UpdateAsync(int id, UpdateCompanyInformationRequestDto dto);
        Task DeleteAsync(int id);
    }
}
