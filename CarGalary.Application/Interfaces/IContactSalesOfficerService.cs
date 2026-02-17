
using CarGalary.Application.Dtos.ContactSalesOfficer.Command;
using CarGalary.Application.Dtos.ContactSalesOfficer.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IContactSalesOfficerService
    {
        Task<List<ContactSalesOfficerResponseDto>> GetAllAsync();
        Task<ContactSalesOfficerResponseDto?> GetByIdAsync(int id);
        Task<ContactSalesOfficerResponseDto> CreateAsync(CreateContactSalesOfficerRequestDto dto);
        Task UpdateAsync(int id, UpdateContactSalesOfficerRequestDto dto);
        Task DeleteAsync(int id);
    }
}
