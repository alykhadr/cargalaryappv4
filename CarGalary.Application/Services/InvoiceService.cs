using AutoMapper;
using CarGalary.Application.Dtos.Invoice.Command;
using CarGalary.Application.Dtos.Invoice.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Application.Utilities;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private const int LowStockThreshold = 2;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public InvoiceService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<InvoiceResponseDto>> GetAllAsync()
        {
            var userBranchId = GetCurrentUserBranchId();
            var items = userBranchId.HasValue
                ? await _unitOfWork.Invoices.GetAllByBranchAsync(userBranchId.Value)
                : await _unitOfWork.Invoices.GetAllAsync();

            var dtos = _mapper.Map<List<InvoiceResponseDto>>(items);
            foreach (var dto in dtos)
            {
                dto.ZatcaQrCode = items.FirstOrDefault(x => x.Id == dto.Id)?.ZatcaQrCode;
            }

            return dtos;
        }

        public async Task<InvoiceResponseDto> GetByIdAsync(int id)
        {
            var userBranchId = GetCurrentUserBranchId();
            var invoice = userBranchId.HasValue
                ? await _unitOfWork.Invoices.GetByIdAsync(id, userBranchId.Value)
                : await _unitOfWork.Invoices.GetByIdAsync(id);

            if (invoice == null || !invoice.IsAvailable)
            {
                throw new KeyNotFoundException($"Invoice not found for id #{id}");
            }

            var dto = _mapper.Map<InvoiceResponseDto>(invoice);
            dto.ZatcaQrCode = invoice.ZatcaQrCode;

            if (string.IsNullOrWhiteSpace(dto.ZatcaQrCode))
            {
                dto.ZatcaQrCode = await TryBuildZatcaQrCodeAsync(invoice);
            }

            return dto;
        }

        public async Task<InvoiceCreateResultDto> CreateWithStockAsync(CreateInvoiceRequestDto dto)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var auditUser = await GetAuditUserNameAsync();
                var auditTimestamp = DateTime.UtcNow;
                ValidateInvoiceDates(dto.IssueDate, dto.DueDate);
                EnsureBranchAccess(dto.BranchId);
                await EnsureBranchExistsAsync(dto.BranchId);
                await EnsureLookupExistsAsync("PAYMENT_METHOD", dto.PaymentMethod);
                await EnsureInvoiceNumberAvailableAsync(dto.InvoiceNumber);

                if (dto.UserId.HasValue && !await _unitOfWork.Invoices.UserExistsAsync(dto.UserId.Value))
                {
                    throw new ArgumentException("UserId is invalid");
                }

                var details = await BuildValidatedDetailsAsync(dto.BranchId, dto.Details
                    .Select(item => new InvoiceItemCalculationInput
                    {
                        CarId = item.CarId,
                        ColorId = item.ColorId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        DiscountAmount = item.DiscountAmount,
                        VatAmount = item.VatAmount,
                        Notes = item.Notes
                    })
                    .ToList());

                var totals = CalculateTotals(details, dto.ShippingFee, dto.ExtraDiscount);
                var entity = _mapper.Map<Invoice>(dto);
                entity.Subtotal = totals.Subtotal;
                entity.VatTotal = totals.VatTotal;
                entity.ShippingFee = totals.ShippingFee;
                entity.ExtraDiscount = totals.ExtraDiscount;
                entity.GrandTotal = totals.GrandTotal;
                entity.ZatcaQrCode = await TryBuildZatcaQrCodeAsync(entity);
                entity.CreatedAt = auditTimestamp;
                entity.CreatedBy = auditUser;
                entity.UpdatedAt = auditTimestamp;
                entity.UpdatedBy = auditUser;
                entity.Details = details.Select(detail => MapToEntity(detail, auditUser, auditTimestamp, setUpdatedAudit: true)).ToList();

                await _unitOfWork.Invoices.CreateAsync(entity);

                var lowStockAlerts = await ApplyStockAdjustmentsAsync(details);

                await _unitOfWork.SaveChangesAsync();

                var created = await GetByIdAsync(entity.Id);
                foreach (var alert in lowStockAlerts)
                {
                    alert.InvoiceId = created.Id;
                    alert.InvoiceNumber = created.InvoiceNumber;
                }

                return new InvoiceCreateResultDto
                {
                    Invoice = created,
                    LowStockAlerts = lowStockAlerts
                };
            });
        }

        public async Task<InvoiceResponseDto> CreateAsync(CreateInvoiceRequestDto dto)
        {
            var result = await CreateWithStockAsync(dto);
            return result.Invoice;
        }

        public async Task<InvoiceResponseDto> UpdateAsync(int id, UpdateInvoiceRequestDto dto)
        {
            var auditUser = await GetAuditUserNameAsync();
            var auditTimestamp = DateTime.UtcNow;
            ValidateInvoiceDates(dto.IssueDate, dto.DueDate);
            EnsureBranchAccess(dto.BranchId);
            await EnsureBranchExistsAsync(dto.BranchId);
            await EnsureLookupExistsAsync("PAYMENT_METHOD", dto.PaymentMethod);
            await EnsureInvoiceNumberAvailableAsync(dto.InvoiceNumber, id);

            var userBranchId = GetCurrentUserBranchId();
            var invoice = userBranchId.HasValue
                ? await _unitOfWork.Invoices.GetByIdForUpdateAsync(id, userBranchId.Value)
                : await _unitOfWork.Invoices.GetByIdForUpdateAsync(id);

            if (invoice == null || !invoice.IsAvailable)
            {
                throw new KeyNotFoundException($"Invoice not found for id #{id}");
            }

            if (dto.UserId.HasValue && !await _unitOfWork.Invoices.UserExistsAsync(dto.UserId.Value))
            {
                throw new ArgumentException("UserId is invalid");
            }

            var details = await BuildValidatedDetailsAsync(dto.BranchId, dto.Details
                .Select(item => new InvoiceItemCalculationInput
                {
                    CarId = item.CarId,
                    ColorId = item.ColorId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    DiscountAmount = item.DiscountAmount,
                    VatAmount = item.VatAmount,
                    Notes = item.Notes
                })
                .ToList());

            var totals = CalculateTotals(details, dto.ShippingFee, dto.ExtraDiscount);

            invoice.UserId = dto.UserId;
            invoice.BranchId = dto.BranchId;
            invoice.PaymentMethod = dto.PaymentMethod;
            invoice.InvoiceNumber = dto.InvoiceNumber.Trim();
            invoice.IssueDate = dto.IssueDate;
            invoice.DueDate = dto.DueDate;
            invoice.CustomerName = dto.CustomerName.Trim();
            invoice.CustomerPhone = dto.CustomerPhone.Trim();
            invoice.CustomerEmail = string.IsNullOrWhiteSpace(dto.CustomerEmail) ? null : dto.CustomerEmail.Trim();
            invoice.CustomerAddress = string.IsNullOrWhiteSpace(dto.CustomerAddress) ? null : dto.CustomerAddress.Trim();
            invoice.Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();
            invoice.Subtotal = totals.Subtotal;
            invoice.VatTotal = totals.VatTotal;
            invoice.ShippingFee = totals.ShippingFee;
            invoice.ExtraDiscount = totals.ExtraDiscount;
            invoice.GrandTotal = totals.GrandTotal;
            invoice.ZatcaQrCode = await TryBuildZatcaQrCodeAsync(invoice);
            invoice.IsAvailable = dto.IsAvailable ?? invoice.IsAvailable;
            invoice.UpdatedAt = auditTimestamp;
            invoice.UpdatedBy = auditUser;

            invoice.Details.Clear();
            foreach (var detail in details.Select(detail => MapToEntity(detail, auditUser, auditTimestamp, setUpdatedAudit: true)))
            {
                invoice.Details.Add(detail);
            }

            await _unitOfWork.SaveChangesAsync();
            return await GetByIdAsync(invoice.Id);
        }

        public async Task DeleteAsync(int id)
        {
            var auditUser = await GetAuditUserNameAsync();
            var auditTimestamp = DateTime.UtcNow;
            var userBranchId = GetCurrentUserBranchId();
            var invoice = userBranchId.HasValue
                ? await _unitOfWork.Invoices.GetByIdForUpdateAsync(id, userBranchId.Value)
                : await _unitOfWork.Invoices.GetByIdForUpdateAsync(id);

            if (invoice == null || !invoice.IsAvailable)
            {
                throw new KeyNotFoundException($"Invoice not found for id #{id}");
            }

            invoice.IsAvailable = false;
            invoice.UpdatedAt = auditTimestamp;
            invoice.UpdatedBy = auditUser;
            foreach (var detail in invoice.Details)
            {
                detail.IsAvailable = false;
                detail.UpdatedAt = auditTimestamp;
                detail.UpdatedBy = auditUser;
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<List<InvoiceItemCalculationResult>> BuildValidatedDetailsAsync(
            int branchId,
            List<InvoiceItemCalculationInput> items)
        {
            if (items.Count == 0)
            {
                throw new ArgumentException("At least one invoice detail is required");
            }

            var results = new List<InvoiceItemCalculationResult>();
            foreach (var item in items)
            {
                var car = await _unitOfWork.Cars.GetByIdAsync(item.CarId);
                if (car == null || !car.IsAvailable)
                {
                    throw new ArgumentException("CarId is invalid");
                }

                if (car.BranchId != branchId)
                {
                    throw new UnauthorizedAccessException("Selected car does not belong to the invoice branch");
                }

                if (item.ColorId.HasValue)
                {
                    var selectedCarColor = await _unitOfWork.CarCarColors.GetByIdAsync(item.CarId, item.ColorId.Value);
                    if (selectedCarColor == null || !selectedCarColor.IsAvailable)
                    {
                        throw new ArgumentException("ColorId is invalid for selected car");
                    }

                    if (selectedCarColor.StockQuantity.HasValue && selectedCarColor.StockQuantity.Value <= 0)
                    {
                        throw new ArgumentException("Selected car color is out of stock");
                    }
                }

                var subtotal = item.Quantity * item.UnitPrice;
                var discount = item.DiscountAmount < 0 ? 0 : item.DiscountAmount;
                if (discount > subtotal)
                {
                    discount = subtotal;
                }

                var vat = item.VatAmount < 0 ? 0 : item.VatAmount;
                var total = subtotal - discount + vat;

                results.Add(new InvoiceItemCalculationResult
                {
                    CarId = item.CarId,
                    CarNameAr = car.NameAr,
                    CarNameEn = car.NameEn,
                    ColorId = item.ColorId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    DiscountAmount = discount,
                    VatAmount = vat,
                    TotalAmount = total,
                    Notes = string.IsNullOrWhiteSpace(item.Notes) ? null : item.Notes.Trim()
                });
            }

            return results;
        }

        private async Task<List<InvoiceLowStockAlertDto>> ApplyStockAdjustmentsAsync(List<InvoiceItemCalculationResult> details)
        {
            var alerts = new List<InvoiceLowStockAlertDto>();

            foreach (var group in details
                         .Where(x => x.ColorId.HasValue)
                         .GroupBy(x => new { x.CarId, ColorId = x.ColorId!.Value }))
            {
                var selectedCarColor = await _unitOfWork.CarCarColors.GetByIdAsync(group.Key.CarId, group.Key.ColorId);
                if (selectedCarColor == null || !selectedCarColor.IsAvailable)
                {
                    throw new ArgumentException("ColorId is invalid for selected car");
                }

                if (!selectedCarColor.StockQuantity.HasValue)
                {
                    continue;
                }

                var requestedQuantity = group.Sum(x => x.Quantity);
                var availableQuantity = selectedCarColor.StockQuantity.Value;
                if (requestedQuantity > availableQuantity)
                {
                    throw new ArgumentException($"Insufficient stock for the selected car color. Available quantity: {availableQuantity}");
                }

                var remainingQuantity = availableQuantity - requestedQuantity;
                selectedCarColor.StockQuantity = remainingQuantity;
                selectedCarColor.UpdatedAt = DateTime.UtcNow;

                if (remainingQuantity <= LowStockThreshold)
                {
                    var sample = group.First();
                    alerts.Add(new InvoiceLowStockAlertDto
                    {
                        CarId = sample.CarId,
                        CarNameAr = sample.CarNameAr,
                        CarNameEn = sample.CarNameEn,
                        ColorId = selectedCarColor.ColorId,
                        ColorNameAr = selectedCarColor.Color?.ColorNameAr,
                        ColorNameEn = selectedCarColor.Color?.ColorNameEn,
                        RemainingStockQuantity = remainingQuantity,
                        ThresholdQuantity = LowStockThreshold
                    });
                }
            }

            return alerts;
        }

        private static InvoiceDetail MapToEntity(
            InvoiceItemCalculationResult item,
            string? auditUser,
            DateTime auditTimestamp,
            bool setUpdatedAudit)
        {
            return new InvoiceDetail
            {
                CarId = item.CarId,
                ColorId = item.ColorId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount,
                VatAmount = item.VatAmount,
                TotalAmount = item.TotalAmount,
                Notes = item.Notes,
                CreatedAt = auditTimestamp,
                CreatedBy = auditUser,
                UpdatedAt = setUpdatedAudit ? auditTimestamp : null,
                UpdatedBy = setUpdatedAudit ? auditUser : null
            };
        }

        private async Task<string?> TryBuildZatcaQrCodeAsync(Invoice invoice)
        {
            var companyInfo = (await _unitOfWork.CompanyInformations.GetAllAsync())
                .FirstOrDefault(x => x.IsAvailable);

            if (companyInfo == null)
            {
                return null;
            }

            var sellerName = FirstNonEmpty(companyInfo.CompanyNameAr, companyInfo.CompanyNameEn);
            var vatRegistrationNumber = companyInfo.VatRegistrationNumber?.Trim();

            if (string.IsNullOrWhiteSpace(sellerName) || string.IsNullOrWhiteSpace(vatRegistrationNumber))
            {
                return null;
            }

            return ZatcaQrCodeBuilder.BuildPhaseOnePayload(
                sellerName,
                vatRegistrationNumber,
                invoice.IssueDate,
                invoice.GrandTotal,
                invoice.VatTotal);
        }

        private static string? FirstNonEmpty(params string?[] values)
        {
            return values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value))?.Trim();
        }

        private static InvoiceTotals CalculateTotals(
            List<InvoiceItemCalculationResult> details,
            decimal shippingFee,
            decimal extraDiscount)
        {
            var normalizedShipping = shippingFee < 0 ? 0 : shippingFee;
            var subtotal = details.Sum(item => item.Quantity * item.UnitPrice);
            var lineDiscounts = details.Sum(item => item.DiscountAmount);
            var vatTotal = details.Sum(item => item.VatAmount);
            var normalizedExtraDiscount = extraDiscount < 0 ? 0 : extraDiscount;
            if (normalizedExtraDiscount > subtotal - lineDiscounts)
            {
                normalizedExtraDiscount = Math.Max(0, subtotal - lineDiscounts);
            }

            var grandTotal = subtotal - lineDiscounts - normalizedExtraDiscount + vatTotal + normalizedShipping;

            return new InvoiceTotals
            {
                Subtotal = subtotal,
                VatTotal = vatTotal,
                ShippingFee = normalizedShipping,
                ExtraDiscount = normalizedExtraDiscount,
                GrandTotal = grandTotal < 0 ? 0 : grandTotal
            };
        }

        private int? GetCurrentUserBranchId()
        {
            return _currentUserService.BranchId;
        }

        private async Task<string?> GetAuditUserNameAsync()
        {
            var currentUserId = _currentUserService.UserId;
            if (!string.IsNullOrWhiteSpace(currentUserId))
            {
                var user = await _unitOfWork.identities.GetUserByIdAsync(currentUserId);
                var fullName = FirstNonEmpty(user?.FullNameAr, user?.FullNameEn);
                if (!string.IsNullOrWhiteSpace(fullName))
                {
                    return fullName;
                }
            }

            return FirstNonEmpty(_currentUserService.UserName, _currentUserService.Email, _currentUserService.UserId);
        }

        private void EnsureBranchAccess(int branchId)
        {
            var userBranchId = GetCurrentUserBranchId();
            if (userBranchId.HasValue && userBranchId.Value != branchId)
            {
                throw new UnauthorizedAccessException("You are not allowed to access data outside your branch");
            }
        }

        private async Task EnsureBranchExistsAsync(int branchId)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(branchId);
            if (branch == null || !branch.IsAvailable)
            {
                throw new ArgumentException("BranchId is invalid");
            }
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

        private async Task EnsureInvoiceNumberAvailableAsync(string invoiceNumber, int? excludeId = null)
        {
            var normalized = invoiceNumber?.Trim();
            if (string.IsNullOrWhiteSpace(normalized))
            {
                throw new ArgumentException("InvoiceNumber is required");
            }

            if (await _unitOfWork.Invoices.InvoiceNumberExistsAsync(normalized, excludeId))
            {
                throw new ArgumentException("InvoiceNumber already exists");
            }
        }

        private static void ValidateInvoiceDates(DateTime issueDate, DateTime dueDate)
        {
            if (dueDate < issueDate)
            {
                throw new ArgumentException("DueDate must be greater than or equal to IssueDate");
            }
        }

        private sealed class InvoiceItemCalculationInput
        {
            public int CarId { get; set; }
            public int? ColorId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal DiscountAmount { get; set; }
            public decimal VatAmount { get; set; }
            public string? Notes { get; set; }
        }

        private sealed class InvoiceItemCalculationResult
        {
            public int CarId { get; set; }
            public string? CarNameAr { get; set; }
            public string? CarNameEn { get; set; }
            public int? ColorId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal DiscountAmount { get; set; }
            public decimal VatAmount { get; set; }
            public decimal TotalAmount { get; set; }
            public string? Notes { get; set; }
        }

        private sealed class InvoiceTotals
        {
            public decimal Subtotal { get; set; }
            public decimal VatTotal { get; set; }
            public decimal ShippingFee { get; set; }
            public decimal ExtraDiscount { get; set; }
            public decimal GrandTotal { get; set; }
        }
    }
}
