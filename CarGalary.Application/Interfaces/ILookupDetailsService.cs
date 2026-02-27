using CarGalary.Application.Dtos.Lookup;

namespace CarGalary.Application.Interfaces
{
    public interface ILookupDetailsService
    {
        Task<List<LookupDetailResponseDto>> GetByMasterCodeAsync(string masterCode);
    }
}
