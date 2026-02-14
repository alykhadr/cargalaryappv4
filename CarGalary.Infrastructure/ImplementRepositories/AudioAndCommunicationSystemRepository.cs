using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class AudioAndCommunicationSystemRepository : IAudioAndCommunicationSystemRepository
    {
        private readonly ApplicationDbContext _context;

        public AudioAndCommunicationSystemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AudioAndCommunicationSystem>> GetAllAsync()
        {
            return await _context.AudioAndCommunicationSystems
                .Where(x => x.IsAvailable)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AudioAndCommunicationSystem?> GetByIdAsync(int id)
        {
            // Keep behavior consistent with other repos: filter by IsAvailable.
            var all = await GetAllAsync();
            return all.FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<AudioAndCommunicationSystem>> GetByCarIdAsync(int carId)
        {
            return await _context.AudioAndCommunicationSystems
                .Where(x => x.IsAvailable && x.CarId == carId)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task CreateAsync(AudioAndCommunicationSystem entity)
        {
            _context.AudioAndCommunicationSystems.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(AudioAndCommunicationSystem entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(AudioAndCommunicationSystem entity)
        {
            entity.IsAvailable = false;
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}

