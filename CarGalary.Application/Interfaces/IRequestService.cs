using CarGalary.Application.Dtos.Request.Command;
using CarGalary.Application.Dtos.Request.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IRequestService
    {
        Task<List<RequestResponseDto>> GetAllAsync();
        Task<RequestNotificationsResponseDto> GetNotificationsAsync(int take = 10);
        Task<RequestResponseDto> GetByIdAsync(int id);
        Task<RequestResponseDto> CreateAsync(CreateRequestDto dto);
        Task<RequestResponseDto> UpdateStatusAsync(int requestId, UpdateRequestStatusDto dto);
        Task<List<RequestHistoryResponseDto>> GetHistoryAsync(int requestId);
    }
}
