using AutoMapper;
using CarGalary.Application.Dtos.ServiceType.Command;
using CarGalary.Application.Dtos.ServiceType.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IUnitOfWork _unitOfWork; private readonly IMapper _mapper;
        public ServiceTypeService(IUnitOfWork unitOfWork,IMapper mapper){_unitOfWork=unitOfWork;_mapper=mapper;}
        public async Task<List<ServiceTypeResponseDto>> GetAllAsync(){var i=await _unitOfWork.ServiceTypes.GetAllAsync(); return _mapper.Map<List<ServiceTypeResponseDto>>(i);} 
        public async Task<ServiceTypeResponseDto?> GetByIdAsync(int id){var i=await _unitOfWork.ServiceTypes.GetByIdAsync(id); return i==null?null:_mapper.Map<ServiceTypeResponseDto>(i);} 
        public async Task<ServiceTypeResponseDto> CreateAsync(CreateServiceTypeRequestDto dto){var e=_mapper.Map<ServiceType>(dto); e.CreatedAt=DateTime.UtcNow; await _unitOfWork.ServiceTypes.CreateAsync(e); await _unitOfWork.SaveChangesAsync(); return _mapper.Map<ServiceTypeResponseDto>(e);} 
        public async Task UpdateAsync(int id, UpdateServiceTypeRequestDto dto){var e=await _unitOfWork.ServiceTypes.GetByIdAsync(id); if(e==null) throw new Exception("ServiceType not found"); if(dto.IsAvailable==null) dto.IsAvailable=e.IsAvailable; _mapper.Map(dto,e); await _unitOfWork.ServiceTypes.UpdateAsync(e); await _unitOfWork.SaveChangesAsync();}
        public async Task DeleteAsync(int id){var e=await _unitOfWork.ServiceTypes.GetByIdAsync(id); if(e==null) throw new Exception("ServiceType not found"); await _unitOfWork.ServiceTypes.DeleteAsync(e); await _unitOfWork.SaveChangesAsync();}
    }
}
