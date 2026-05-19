using CarGalary.Application.Dtos.PrivacyPolicy.Command;
using CarGalary.Application.Dtos.PrivacyPolicy.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IPrivacyPolicyService
    {
        Task<List<PrivacyPolicyResponseDto>> GetAllAsync();
        Task<PrivacyPolicyResponseDto?> GetFirstAsync();
        Task<PrivacyPolicyResponseDto?> GetByIdAsync(int id);
        Task<PrivacyPolicyResponseDto> CreateAsync(CreatePrivacyPolicyRequestDto dto);
        Task UpdateAsync(int id, UpdatePrivacyPolicyRequestDto dto);
        Task DeleteAsync(int id);
    }
}
