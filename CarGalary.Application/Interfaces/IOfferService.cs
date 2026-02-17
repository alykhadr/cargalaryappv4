using CarGalary.Application.Dtos.Offer.Command;
using CarGalary.Application.Dtos.Offer.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IOfferService
    {
        Task<List<OfferResponseDto>> GetAllAsync();
        Task<OfferResponseDto?> GetByIdAsync(int id);
        Task<OfferResponseDto> CreateAsync(CreateOfferRequestDto dto);
        Task UpdateAsync(int id, UpdateOfferRequestDto dto);
        Task DeleteAsync(int id);
    }
}
