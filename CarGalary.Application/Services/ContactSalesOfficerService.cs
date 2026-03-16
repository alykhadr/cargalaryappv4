using AutoMapper;
using CarGalary.Application.Dtos.ContactSalesOfficer.Command;
using CarGalary.Application.Dtos.ContactSalesOfficer.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;
using System.Globalization;

namespace CarGalary.Application.Services
{
    public class ContactSalesOfficerService : IContactSalesOfficerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactSalesOfficerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ContactSalesOfficerResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.ContactSalesOfficers.GetAllAsync();
            var response = _mapper.Map<List<ContactSalesOfficerResponseDto>>(items);
            await PopulateContactTypeNamesAsync(response);
            return response;
        }

        public async Task<ContactSalesOfficerResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.ContactSalesOfficers.GetByIdAsync(id);
            if (item == null)
            {
                return null;
            }

            var response = _mapper.Map<ContactSalesOfficerResponseDto>(item);
            await PopulateContactTypeNamesAsync(new List<ContactSalesOfficerResponseDto> { response });
            return response;
        }

        public async Task<ContactSalesOfficerResponseDto> CreateAsync(CreateContactSalesOfficerRequestDto dto)
        {
            var contactTypeLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CONTACT_TYPE", dto.ContactType.ToString());
            if (contactTypeLookup == null)
            {
                throw new Exception("ContactType is invalid");
            }

            var entity = _mapper.Map<ContactSalesOfficer>(dto);
            entity.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.ContactSalesOfficers.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<ContactSalesOfficerResponseDto>(entity);
            await PopulateContactTypeNamesAsync(new List<ContactSalesOfficerResponseDto> { response });
            return response;
        }

        public async Task UpdateAsync(int id, UpdateContactSalesOfficerRequestDto dto)
        {
            var existing = await _unitOfWork.ContactSalesOfficers.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("ContactSalesOfficer not found");
            }

            var contactTypeLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CONTACT_TYPE", dto.ContactType.ToString());
            if (contactTypeLookup == null)
            {
                throw new Exception("ContactType is invalid");
            }

            _mapper.Map(dto, existing);
            await _unitOfWork.ContactSalesOfficers.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.ContactSalesOfficers.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("ContactSalesOfficer not found");
            }

            await _unitOfWork.ContactSalesOfficers.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task PopulateContactTypeNamesAsync(List<ContactSalesOfficerResponseDto> responses)
        {
            if (responses.Count == 0)
            {
                return;
            }

            var lookupMap = await BuildLookupMapAsync("CONTACT_TYPE");
            foreach (var item in responses)
            {
                var key = item.ContactType.ToString(CultureInfo.InvariantCulture);
                if (lookupMap.TryGetValue(key, out var lookup))
                {
                    item.ContactTypeNameAr = lookup.NameAr;
                    item.ContactTypeNameEn = lookup.NameEn;
                }
            }
        }

        private async Task<Dictionary<string, LookupDetails>> BuildLookupMapAsync(string masterCode)
        {
            var values = await _unitOfWork.LookupDetails.GetByMasterCodeAsync(masterCode);
            return values
                .Where(x => !string.IsNullOrWhiteSpace(x.DetailCode))
                .GroupBy(x => x.DetailCode.Trim())
                .ToDictionary(x => x.Key, x => x.First());
        }
    }
}
