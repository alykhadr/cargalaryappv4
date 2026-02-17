using CarGalary.Application.Dtos.MemberService.Command;
using CarGalary.Application.Dtos.MemberService.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IMemberServiceService
    {
        Task<List<MemberServiceResponseDto>> GetAllAsync();
        Task<MemberServiceResponseDto?> GetByIdAsync(int id);
        Task<MemberServiceResponseDto> CreateAsync(CreateMemberServiceRequestDto dto);
        Task UpdateAsync(int id, UpdateMemberServiceRequestDto dto);
        Task DeleteAsync(int id);
    }
}
