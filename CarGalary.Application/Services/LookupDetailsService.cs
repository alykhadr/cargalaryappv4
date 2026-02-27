using CarGalary.Application.Dtos.Lookup;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class LookupDetailsService : ILookupDetailsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LookupDetailsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LookupDetailResponseDto>> GetByMasterCodeAsync(string masterCode)
        {
            var rows = await _unitOfWork.LookupDetails.GetByMasterCodeAsync(masterCode);
            return rows.Select(x => new LookupDetailResponseDto
            {
                Id = x.Id,
                MasterCode = x.MasterCode,
                DetailCode = x.DetailCode,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                MappedCode = x.MappedCode
            }).ToList();
        }
    }
}
