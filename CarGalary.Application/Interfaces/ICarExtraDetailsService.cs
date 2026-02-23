
using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Query;
using CarGalary.Application.Dtos.CarExtraDetails.Command;

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

