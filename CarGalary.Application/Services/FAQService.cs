using AutoMapper;
using CarGalary.Application.Dtos.FAQ.Command;
using CarGalary.Application.Dtos.FAQ.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class FAQService : IFAQService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FAQService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<FAQResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.FAQs.GetAllAsync();
            return _mapper.Map<List<FAQResponseDto>>(items);
        }

        public async Task<FAQResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.FAQs.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<FAQResponseDto>(item);
        }

        public async Task<FAQResponseDto> CreateAsync(CreateFAQRequestDto dto)
        {
            var entity = _mapper.Map<FAQ>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            await _unitOfWork.FAQs.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<FAQResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateFAQRequestDto dto)
        {
            var existing = await _unitOfWork.FAQs.GetByIdAsync(id);
            if (existing == null) throw new Exception("FAQ not found");
            if (dto.IsAvailable == null) dto.IsAvailable = existing.IsAvailable;
            _mapper.Map(dto, existing);
            await _unitOfWork.FAQs.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.FAQs.GetByIdAsync(id);
            if (existing == null) throw new Exception("FAQ not found");
            await _unitOfWork.FAQs.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
