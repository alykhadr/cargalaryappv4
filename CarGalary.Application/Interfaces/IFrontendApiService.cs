using CarGalary.Application.Dtos.Frontend;

namespace CarGalary.Application.Interfaces
{
    public interface IFrontendApiService
    {
        Task<FrontendCarsResponseDto> GetCarsAsync(FrontendCarQueryDto query);
        Task<FrontendCarDto?> GetCarAsync(int id);
        Task<List<FrontendCarDto>> GetWishlistAsync(Guid userId);
        Task<bool> IsWishlistedAsync(Guid userId, int carId);
        Task AddToWishlistAsync(Guid userId, int carId);
        Task RemoveFromWishlistAsync(Guid userId, int carId);
        Task<List<FrontendInquiryWithCarDto>> GetInquiriesAsync(Guid userId);
        Task<FrontendInquiryDto> CreateInquiryAsync(Guid userId, string email, FrontendCreateInquiryRequestDto request);
    }
}
