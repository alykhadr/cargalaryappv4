using AutoMapper;
using CarGalary.Application.Dtos.Request.Command;
using CarGalary.Application.Dtos.Request.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public RequestService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<RequestResponseDto>> GetAllAsync()
        {
            var userBranchId = GetCurrentUserBranchId();
            var items = userBranchId.HasValue
                ? await _unitOfWork.Requests.GetAllByBranchAsync(userBranchId.Value)
                : await _unitOfWork.Requests.GetAllAsync();
            return _mapper.Map<List<RequestResponseDto>>(items);
        }

        public async Task<RequestNotificationsResponseDto> GetNotificationsAsync(int take = 10)
        {
            var userBranchId = GetCurrentUserBranchId();
            var items = userBranchId.HasValue
                ? await _unitOfWork.Requests.GetAllByBranchAsync(userBranchId.Value)
                : await _unitOfWork.Requests.GetAllAsync();

            var requestStatusLookups = await _unitOfWork.LookupDetails.GetByMasterCodeAsync("REQUEST_STATUS");
            var newStatusIds = requestStatusLookups
                .Where(x => string.Equals(x.DetailCode, "1", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Id)
                .ToHashSet();

            var newRequests = items
                .Where(x => x.IsAvailable && (newStatusIds.Count == 0 || newStatusIds.Contains(x.CurrentStatus)))
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            var notifications = newRequests
                .Take(Math.Max(1, take))
                .Select(x => new RequestNotificationItemDto
                {
                    Id = x.Id,
                    CarName = !string.IsNullOrWhiteSpace(x.Car?.NameEn)
                        ? x.Car.NameEn
                        : (x.Car?.NameAr ?? $"Car #{x.CarId}"),
                    CarImageUrl = x.Car?.CarImages
                        .Where(i => i.IsAvailable && !string.IsNullOrWhiteSpace(i.ImageUrl))
                        .OrderByDescending(i => i.IsPrimary)
                        .ThenByDescending(i => i.CreatedAt)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault(),
                    CreatedDate = x.CreatedAt
                })
                .ToList();

            return new RequestNotificationsResponseDto
            {
                Count = newRequests.Count,
                Items = notifications
            };
        }

        public async Task<RequestResponseDto> GetByIdAsync(int id)
        {
            var userBranchId = GetCurrentUserBranchId();
            var request = userBranchId.HasValue
                ? await _unitOfWork.Requests.GetByIdAsync(id, userBranchId.Value)
                : await _unitOfWork.Requests.GetByIdAsync(id);
            if (request == null || !request.IsAvailable)
            {
                throw new KeyNotFoundException($"Request not found for id #{id}");
            }

            return _mapper.Map<RequestResponseDto>(request);
        }

        public async Task<List<RequestHistoryResponseDto>> GetHistoryAsync(int requestId)
        {
            var userBranchId = GetCurrentUserBranchId();
            var request = userBranchId.HasValue
                ? await _unitOfWork.Requests.GetByIdAsync(requestId, userBranchId.Value)
                : await _unitOfWork.Requests.GetByIdAsync(requestId);
            if (request == null || !request.IsAvailable)
            {
                throw new KeyNotFoundException($"Request not found for id #{requestId}");
            }

            var historyItems = await _unitOfWork.RequestHistories.GetByRequestIdAsync(requestId);
            return _mapper.Map<List<RequestHistoryResponseDto>>(historyItems);
        }

        public async Task<RequestResponseDto> CreateAsync(CreateRequestDto dto)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(dto.CarId);
            if (car == null || !car.IsAvailable)
            {
                throw new ArgumentException("CarId is invalid");
            }

            var selectedCarColor = await _unitOfWork.CarCarColors.GetByIdAsync(dto.CarId, dto.ColorId);
            if (selectedCarColor == null || !selectedCarColor.IsAvailable)
            {
                throw new ArgumentException("ColorId is invalid for selected car");
            }

            EnsureBranchAccess(car.BranchId);

            await EnsureLookupExistsAsync("PAYMENT_METHOD", dto.PaymentMethod);
            await EnsureLookupExistsAsync("REGION", dto.RegionId);
            await EnsureLookupExistsAsync("CITY", dto.CityId);
            await EnsureLookupExistsAsync("VEHICLE_OWNER_TYPE", dto.VehicleOwnerType);

            if (dto.UserId.HasValue)
            {
                var userId = dto.UserId.Value;
                if (!await _unitOfWork.Requests.UserExistsAsync(userId))
                {
                    throw new ArgumentException("UserId is invalid");
                }

                if (await _unitOfWork.Requests.UserHasRequestAsync(userId))
                {
                    throw new ArgumentException("This user already has a request");
                }
            }

            var entity = _mapper.Map<Request>(dto);
            var now = DateTime.UtcNow;
            entity.CreatedAt = DateTime.UtcNow;
            entity.CurrentStatus = await ResolveLookupIdAsync("REQUEST_STATUS", 1);
            entity.CurrentStatusDate = now;

            entity.Histories.Add(new RequestHistory
            {
                Status = entity.CurrentStatus,
                StatusDate = now,
                Notes = "Request created with status New",
                CreatedAt = now
            });

            await _unitOfWork.Requests.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RequestResponseDto>(entity);
        }

        public async Task<RequestResponseDto> UpdateStatusAsync(int requestId, UpdateRequestStatusDto dto)
        {
            var userBranchId = GetCurrentUserBranchId();
            var request = userBranchId.HasValue
                ? await _unitOfWork.Requests.GetByIdForUpdateAsync(requestId, userBranchId.Value)
                : await _unitOfWork.Requests.GetByIdForUpdateAsync(requestId);
            if (request == null || !request.IsAvailable)
            {
                throw new KeyNotFoundException($"Request not found for id #{requestId}");
            }

            await EnsureLookupExistsAsync("REQUEST_STATUS", dto.CurrentStatus);

            if (await IsFinalClosedStatusAsync(request.CurrentStatus))
            {
                throw new ArgumentException("1001");
            }

            if (request.CurrentStatus == dto.CurrentStatus)
            {
                throw new ArgumentException("Request already has this status");
            }

            var duplicatedStatus = await _unitOfWork.RequestHistories
                .ExistsByRequestAndStatusAsync(request.Id, dto.CurrentStatus);
            if (!duplicatedStatus)
            {


                var now = DateTime.UtcNow;
                request.CurrentStatus = dto.CurrentStatus;
                request.CurrentStatusDate = now;
                request.UpdatedAt = now;

                await _unitOfWork.RequestHistories.CreateAsync(new RequestHistory
                {
                    RequestId = request.Id,
                    Status = dto.CurrentStatus,
                    StatusDate = now,
                    Notes = dto.Notes,
                    CreatedAt = now
                });

                await _unitOfWork.SaveChangesAsync();

            }
            var refreshedRequest = userBranchId.HasValue
                ? await _unitOfWork.Requests.GetByIdAsync(requestId, userBranchId.Value)
                : await _unitOfWork.Requests.GetByIdAsync(requestId);

            return _mapper.Map<RequestResponseDto>(refreshedRequest ?? request);
        }

        private int? GetCurrentUserBranchId()
        {
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
            var lookups = await _unitOfWork.LookupDetails.GetByMasterCodeAsync("REQUEST_STATUS");
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
