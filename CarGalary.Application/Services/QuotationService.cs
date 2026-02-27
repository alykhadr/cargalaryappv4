using AutoMapper;
using CarGalary.Application.Dtos.Quotation.Command;
using CarGalary.Application.Dtos.Quotation.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuotationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<QuotationResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.Quotations.GetAllAsync();
            return _mapper.Map<List<QuotationResponseDto>>(items);
        }

        public async Task<QuotationResponseDto> CreateAsync(CreateQuotationRequestDto dto)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(dto.CarId);
            if (car == null || !car.IsAvailable)
            {
                throw new Exception("CarId is invalid");
            }

            await EnsureLookupExistsAsync("PAYMENT_METHOD", dto.PaymentMethod);
            await EnsureLookupExistsAsync("REGION", dto.RegionId);
            await EnsureLookupExistsAsync("CITY", dto.CityId);
            await EnsureLookupExistsAsync("VEHICLE_OWNER_TYPE", dto.VehicleOwnerType);

            if (dto.UserId.HasValue)
            {
                var userId = dto.UserId.Value;
                if (!await _unitOfWork.Quotations.UserExistsAsync(userId))
                {
                    throw new Exception("UserId is invalid");
                }

                if (await _unitOfWork.Quotations.UserHasQuotationAsync(userId))
                {
                    throw new Exception("This user already has a quotation");
                }
            }

            var entity = _mapper.Map<Quotation>(dto);
            entity.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Quotations.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<QuotationResponseDto>(entity);
        }

        private async Task EnsureLookupExistsAsync(string masterCode, int detailCode)
        {
            var lookups = await _unitOfWork.LookupDetails.GetByMasterCodeAsync(masterCode);
            var exists = lookups.Any(x => x.Id == detailCode || x.DetailCode == detailCode.ToString());
            if (!exists)
            {
                throw new Exception($"{masterCode} is invalid");
            }
        }
    }
}
