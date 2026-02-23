using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Command;
using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Query;

namespace CarGalary.Application.Interfaces
{
    public interface ICarExtraDetailsService
    {
        Task<List<CarExtraDetailsResponseDto>> GetAllAsync();
        Task<CarExtraDetailsResponseDto?> GetByIdAsync(int id);
        Task<List<CarExtraDetailsResponseDto>> GetByCarIdAsync(int carId);

        Task<CarExtraDetailsResponseDto> CreateAsync(CreateCarExtraDetailsRequestDto dto);
        Task UpdateAsync(int id, UpdateCarExtraDetailsRequestDto dto);
        Task DeleteAsync(int id);
    }
}

