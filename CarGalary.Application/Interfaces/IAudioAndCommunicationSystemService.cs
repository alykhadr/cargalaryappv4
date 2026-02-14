using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Command;
using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IAudioAndCommunicationSystemService
    {
        Task<List<AudioAndCommunicationSystemResponseDto>> GetAllAsync();
        Task<AudioAndCommunicationSystemResponseDto?> GetByIdAsync(int id);
        Task<List<AudioAndCommunicationSystemResponseDto>> GetByCarIdAsync(int carId);

        Task<AudioAndCommunicationSystemResponseDto> CreateAsync(CreateAudioAndCommunicationSystemRequestDto dto);
        Task UpdateAsync(int id, UpdateAudioAndCommunicationSystemRequestDto dto);
        Task DeleteAsync(int id);
    }
}

