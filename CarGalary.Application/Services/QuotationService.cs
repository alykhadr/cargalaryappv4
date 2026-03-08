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
        private readonly ICurrentUserService _currentUserService;

        public QuotationService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<QuotationResponseDto>> GetAllAsync()
        {
            var userBranchId = GetCurrentUserBranchId();
            var items = userBranchId.HasValue
                ? await _unitOfWork.Quotations.GetAllByBranchAsync(userBranchId.Value)
                : await _unitOfWork.Quotations.GetAllAsync();
            return _mapper.Map<List<QuotationResponseDto>>(items);
        }

        public async Task<QuotationResponseDto> GetByIdAsync(int id)
        {
            var userBranchId = GetCurrentUserBranchId();
            var quotation = userBranchId.HasValue
                ? await _unitOfWork.Quotations.GetByIdAsync(id, userBranchId.Value)
                : await _unitOfWork.Quotations.GetByIdAsync(id);
            if (quotation == null || !quotation.IsAvailable)
            {
                throw new KeyNotFoundException($"Quotation not found for id #{id}");
            }

            return _mapper.Map<QuotationResponseDto>(quotation);
        }

        public async Task<List<QuotationHistoryResponseDto>> GetHistoryAsync(int quotationId)
        {
            var userBranchId = GetCurrentUserBranchId();
            var quotation = userBranchId.HasValue
                ? await _unitOfWork.Quotations.GetByIdAsync(quotationId, userBranchId.Value)
                : await _unitOfWork.Quotations.GetByIdAsync(quotationId);
            if (quotation == null || !quotation.IsAvailable)
            {
                throw new KeyNotFoundException($"Quotation not found for id #{quotationId}");
            }

            var historyItems = await _unitOfWork.QuotationHistories.GetByQuotationIdAsync(quotationId);
            return _mapper.Map<List<QuotationHistoryResponseDto>>(historyItems);
        }

        public async Task<QuotationResponseDto> CreateAsync(CreateQuotationRequestDto dto)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(dto.CarId);
            if (car == null || !car.IsAvailable)
            {
                throw new ArgumentException("CarId is invalid");
            }

            EnsureBranchAccess(car.BranchId);

            await EnsureLookupExistsAsync("PAYMENT_METHOD", dto.PaymentMethod);
            await EnsureLookupExistsAsync("REGION", dto.RegionId);
            await EnsureLookupExistsAsync("CITY", dto.CityId);
            await EnsureLookupExistsAsync("VEHICLE_OWNER_TYPE", dto.VehicleOwnerType);

            if (dto.UserId.HasValue)
            {
                var userId = dto.UserId.Value;
                if (!await _unitOfWork.Quotations.UserExistsAsync(userId))
                {
                    throw new ArgumentException("UserId is invalid");
                }

                if (await _unitOfWork.Quotations.UserHasQuotationAsync(userId))
                {
                    throw new ArgumentException("This user already has a quotation");
                }
            }

            var entity = _mapper.Map<Quotation>(dto);
            var now = DateTime.UtcNow;
            entity.CreatedAt = DateTime.UtcNow;
            entity.CurrentStatus = await ResolveLookupIdAsync("QUOTATION_STATUS", 1);
            entity.CurrentStatusDate = now;

            entity.Histories.Add(new QuotationHistory
            {
                Status = entity.CurrentStatus,
                StatusDate = now,
                Notes = "Quotation created with status New",
                CreatedAt = now
            });

            await _unitOfWork.Quotations.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<QuotationResponseDto>(entity);
        }

        public async Task<QuotationResponseDto> UpdateStatusAsync(int quotationId, UpdateQuotationStatusRequestDto dto)
        {
            var userBranchId = GetCurrentUserBranchId();
            var quotation = userBranchId.HasValue
                ? await _unitOfWork.Quotations.GetByIdForUpdateAsync(quotationId, userBranchId.Value)
                : await _unitOfWork.Quotations.GetByIdForUpdateAsync(quotationId);
            if (quotation == null || !quotation.IsAvailable)
            {
                throw new KeyNotFoundException($"Quotation not found for id #{quotationId}");
            }

            await EnsureLookupExistsAsync("QUOTATION_STATUS", dto.CurrentStatus);

            if (await IsFinalClosedStatusAsync(quotation.CurrentStatus))
            {
                throw new ArgumentException("1001");
            }

            if (quotation.CurrentStatus == dto.CurrentStatus)
            {
                throw new ArgumentException("Quotation already has this status");
            }

            var duplicatedStatus = await _unitOfWork.QuotationHistories
                .ExistsByQuotationAndStatusAsync(quotation.Id, dto.CurrentStatus);
            if (!duplicatedStatus)
            {


                var now = DateTime.UtcNow;
                quotation.CurrentStatus = dto.CurrentStatus;
                quotation.CurrentStatusDate = now;
                quotation.UpdatedAt = now;

                await _unitOfWork.QuotationHistories.CreateAsync(new QuotationHistory
                {
                    QuotationId = quotation.Id,
                    Status = dto.CurrentStatus,
                    StatusDate = now,
                    Notes = dto.Notes,
                    CreatedAt = now
                });

                await _unitOfWork.SaveChangesAsync();

            }
            return _mapper.Map<QuotationResponseDto>(quotation);
        }

        private int? GetCurrentUserBranchId()
        {
            if (_currentUserService.IsInRole("Admin"))
            {
                return null;
            }

            return _currentUserService.BranchId;
        }

        private void EnsureBranchAccess(int branchId)
        {
            var userBranchId = GetCurrentUserBranchId();
            if (userBranchId.HasValue && userBranchId.Value != branchId)
            {
                throw new UnauthorizedAccessException("You are not allowed to access data outside your branch");
            }
        }

        private async Task<int> ResolveLookupIdAsync(string masterCode, int detailCode)
        {
            var lookups = await _unitOfWork.LookupDetails.GetByMasterCodeAsync(masterCode);
            var matched = lookups.FirstOrDefault(x => x.Id == detailCode || x.DetailCode == detailCode.ToString());
            if (matched == null)
            {
                throw new ArgumentException($"{masterCode} is invalid");
            }

            return matched.Id;
        }

        private async Task EnsureLookupExistsAsync(string masterCode, int detailCode)
        {
            var lookups = await _unitOfWork.LookupDetails.GetByMasterCodeAsync(masterCode);
            var exists = lookups.Any(x => x.Id == detailCode || x.DetailCode == detailCode.ToString());
            if (!exists)
            {
                throw new ArgumentException($"{masterCode} is invalid");
            }
        }

        private async Task<bool> IsFinalClosedStatusAsync(int statusId)
        {
            var lookups = await _unitOfWork.LookupDetails.GetByMasterCodeAsync("QUOTATION_STATUS");
            var current = lookups.FirstOrDefault(x => x.Id == statusId);
            if (current == null)
            {
                return false;
            }

            return string.Equals(current.DetailCode, "4", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(current.DetailCode, "5", StringComparison.OrdinalIgnoreCase);
        }
    }
}
