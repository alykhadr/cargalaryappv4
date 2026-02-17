using CarGalary.Application.Dtos.Transmission.Command;
using CarGalary.Application.Dtos.Transmission.Query;

namespace CarGalary.Application.Interfaces
{
    public interface ITransmissionService
    {
        Task<List<TransmissionResponseDto>> GetAllAsync();
        Task<TransmissionResponseDto?> GetByIdAsync(int id);
        Task<TransmissionResponseDto> CreateAsync(CreateTransmissionRequestDto dto);
        Task UpdateAsync(int id, UpdateTransmissionRequestDto dto);
        Task DeleteAsync(int id);
    }
}
