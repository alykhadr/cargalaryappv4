using AutoMapper;
using CarGalary.Application.Dtos.ContactSalesOfficer.Command;
using CarGalary.Application.Dtos.ContactSalesOfficer.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

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
            return _mapper.Map<List<ContactSalesOfficerResponseDto>>(items);
        }

        public async Task<ContactSalesOfficerResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.ContactSalesOfficers.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<ContactSalesOfficerResponseDto>(item);
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

            return _mapper.Map<ContactSalesOfficerResponseDto>(entity);
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
    }
}
