

using CarGalary.Application.Dtos.ContactUs.Command;
using CarGalary.Application.Dtos.ContactUs.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IContactUsService
    {
        Task<List<ContactUsResponseDto>> GetAllAsync();
        Task<ContactUsResponseDto?> GetByIdAsync(int id);
        Task<ContactUsResponseDto> CreateAsync(CreateContactUsRequestDto dto);
        Task UpdateAsync(int id, UpdateContactUsRequestDto dto);
        Task DeleteAsync(int id);
    }
}
