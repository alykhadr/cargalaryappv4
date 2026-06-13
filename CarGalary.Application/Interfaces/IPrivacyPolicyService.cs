using CarGalary.Application.Dtos.PrivacyPolicy.Command;
using CarGalary.Application.Dtos.PrivacyPolicy.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IPrivacyPolicyService
    {
        Task<List<PrivacyPolicyResponseDto>> GetAllAsync();
        Task<List<PrivacyPolicyResponseDto>> GetAvailableAsync();
        Task<PrivacyPolicyResponseDto?> GetByIdAsync(int id);
        Task<PrivacyPolicyResponseDto?> GetLatestAvailableAsync();
        Task<PrivacyPolicyResponseDto> CreateAsync(CreatePrivacyPolicyRequestDto dto);
        Task UpdateAsync(int id, UpdatePrivacyPolicyRequestDto dto);
        Task DeleteAsync(int id);
    }
}
