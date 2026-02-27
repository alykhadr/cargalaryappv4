using CarGalary.Application.Dtos.Department.Command;
using CarGalary.Application.Dtos.Department.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponseDto>> GetAllAsync();
        Task<DepartmentResponseDto> GetByIdAsync(int id);
        Task<DepartmentResponseDto> CreateAsync(CreateDepartmentRequestDto requestDto);
        Task<bool> UpdateAsync(int id, UpdateDepartmentRequestDto requestDto);
        Task<bool> DeleteAsync(int id);
    }
}
