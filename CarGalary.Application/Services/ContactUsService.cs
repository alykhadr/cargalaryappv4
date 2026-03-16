using AutoMapper;
using CarGalary.Application.Dtos.ContactUs.Command;
using CarGalary.Application.Dtos.ContactUs.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;
using System.Globalization;

namespace CarGalary.Application.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactUsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ContactUsResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.ContactUs.GetAllAsync();
            var response = _mapper.Map<List<ContactUsResponseDto>>(items);
            await PopulateContactTypeNamesAsync(response);
            return response;
        }

        public async Task<ContactUsResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.ContactUs.GetByIdAsync(id);
            if (item == null)
            {
                return null;
            }

            var response = _mapper.Map<ContactUsResponseDto>(item);
            await PopulateContactTypeNamesAsync(new List<ContactUsResponseDto> { response });
            return response;
        }

        public async Task<ContactUsResponseDto> CreateAsync(CreateContactUsRequestDto dto)
        {
            var contactTypeLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CONTACT_TYPE", dto.ContactType.ToString());
            if (contactTypeLookup == null)
            {
                throw new Exception("ContactType is invalid");
            }

            var entity = _mapper.Map<ContactUs>(dto);
            entity.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.ContactUs.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<ContactUsResponseDto>(entity);
            await PopulateContactTypeNamesAsync(new List<ContactUsResponseDto> { response });
            return response;
        }

        public async Task UpdateAsync(int id, UpdateContactUsRequestDto dto)
        {
            var existing = await _unitOfWork.ContactUs.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("ContactUs not found");
            }

            var contactTypeLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CONTACT_TYPE", dto.ContactType.ToString());
            if (contactTypeLookup == null)
            {
                throw new Exception("ContactType is invalid");
            }

            if (!dto.IsAvailable)
            {
                dto.IsAvailable = existing.IsAvailable;
            }

            _mapper.Map(dto, existing);
            await _unitOfWork.ContactUs.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.ContactUs.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("ContactUs not found");
            }

            await _unitOfWork.ContactUs.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task PopulateContactTypeNamesAsync(List<ContactUsResponseDto> responses)
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
