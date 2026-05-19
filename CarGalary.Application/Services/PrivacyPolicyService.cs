using AutoMapper;
using CarGalary.Application.Dtos.PrivacyPolicy.Command;
using CarGalary.Application.Dtos.PrivacyPolicy.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class PrivacyPolicyService : IPrivacyPolicyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PrivacyPolicyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<PrivacyPolicyResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.PrivacyPolicies.GetAllAsync();
            return _mapper.Map<List<PrivacyPolicyResponseDto>>(items);
        }

        public async Task<PrivacyPolicyResponseDto?> GetFirstAsync()
        {
            var item = await _unitOfWork.PrivacyPolicies.GetFirstAsync();
            return item == null ? null : _mapper.Map<PrivacyPolicyResponseDto>(item);
        }

        public async Task<PrivacyPolicyResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.PrivacyPolicies.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<PrivacyPolicyResponseDto>(item);
        }

        public async Task<PrivacyPolicyResponseDto> CreateAsync(CreatePrivacyPolicyRequestDto dto)
        {
            var entity = _mapper.Map<PrivacyPolicy>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            await _unitOfWork.PrivacyPolicies.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PrivacyPolicyResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdatePrivacyPolicyRequestDto dto)
        {
            var existing = await _unitOfWork.PrivacyPolicies.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Privacy policy not found");
            }

            if (dto.IsAvailable == null)
            {
                dto.IsAvailable = existing.IsAvailable;
            }

            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.PrivacyPolicies.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.PrivacyPolicies.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Privacy policy not found");
            }

            await _unitOfWork.PrivacyPolicies.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
