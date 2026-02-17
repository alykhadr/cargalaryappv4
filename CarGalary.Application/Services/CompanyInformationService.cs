using AutoMapper;
using CarGalary.Application.Dtos.CompanyInformation.Command;
using CarGalary.Application.Dtos.CompanyInformation.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class CompanyInformationService : ICompanyInformationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompanyInformationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CompanyInformationResponseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.CompanyInformations.GetAllAsync();
            return _mapper.Map<List<CompanyInformationResponseDto>>(items);
        }

        public async Task<CompanyInformationResponseDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.CompanyInformations.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<CompanyInformationResponseDto>(item);
        }

        public async Task<CompanyInformationResponseDto> CreateAsync(CreateCompanyInformationRequestDto dto)
        {
            var entity = _mapper.Map<CompanyInformation>(dto);
            entity.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.CompanyInformations.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CompanyInformationResponseDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateCompanyInformationRequestDto dto)
        {
            var existing = await _unitOfWork.CompanyInformations.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("CompanyInformation not found");
            }

            if (dto.IsAvailable == null)
            {
                dto.IsAvailable = existing.IsAvailable;
            }

            _mapper.Map(dto, existing);
            await _unitOfWork.CompanyInformations.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.CompanyInformations.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("CompanyInformation not found");
            }

            await _unitOfWork.CompanyInformations.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
