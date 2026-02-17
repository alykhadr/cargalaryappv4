using CarGalary.Application.Dtos.Seating.Command;
using CarGalary.Application.Dtos.Seating.Query;

namespace CarGalary.Application.Interfaces
{
    public interface ISeatingService
    {
        Task<List<SeatingResponseDto>> GetAllAsync();
        Task<SeatingResponseDto?> GetByIdAsync(int id);
        Task<SeatingResponseDto> CreateAsync(CreateSeatingRequestDto dto);
        Task UpdateAsync(int id, UpdateSeatingRequestDto dto);
        Task DeleteAsync(int id);
    }
}
