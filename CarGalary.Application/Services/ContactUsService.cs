using AutoMapper;
using CarGalary.Application.Dtos.ContactUs.Command;
using CarGalary.Application.Dtos.ContactUs.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;

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
            return _mapper.Map<List<ContactUsResponseDto>>(items);
        }

        public async Task<ContactUsResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.ContactUs.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<ContactUsResponseDto>(item);
        }

        public async Task<ContactUsResponseDto> CreateAsync(CreateContactUsRequestDto dto)
        {
            var entity = _mapper.Map<ContactUs>(dto);
            entity.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.ContactUs.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ContactUsResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateContactUsRequestDto dto)
        {
            var existing = await _unitOfWork.ContactUs.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("ContactUs not found");
            }

            if (dto.IsAvailable == null)
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
    }
}
