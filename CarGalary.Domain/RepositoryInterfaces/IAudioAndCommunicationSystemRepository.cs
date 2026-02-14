using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IAudioAndCommunicationSystemRepository
    {
        Task<List<AudioAndCommunicationSystem>> GetAllAsync();
        Task<AudioAndCommunicationSystem?> GetByIdAsync(int id);
        Task<List<AudioAndCommunicationSystem>> GetByCarIdAsync(int carId);

        Task CreateAsync(AudioAndCommunicationSystem entity);
        Task UpdateAsync(AudioAndCommunicationSystem entity);
        Task DeleteAsync(AudioAndCommunicationSystem entity);
    }
}

