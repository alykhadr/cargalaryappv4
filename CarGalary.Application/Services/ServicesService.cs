using AutoMapper;
using CarGalary.Application.Dtos.Services.Command;
using CarGalary.Application.Dtos.Services.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IUnitOfWork _unitOfWork; private readonly IMapper _mapper;
        public ServicesService(IUnitOfWork unitOfWork, IMapper mapper) { _unitOfWork = unitOfWork; _mapper = mapper; }
        public async Task<List<ServicesResponseDto>> GetAllAsync() { var i = await _unitOfWork.Services.GetAllAsync(); return _mapper.Map<List<ServicesResponseDto>>(i); }
        public async Task<ServicesResponseDto?> GetByIdAsync(int id) { var i = await _unitOfWork.Services.GetByIdAsync(id); return i == null ? null : _mapper.Map<ServicesResponseDto>(i); }
        public async Task<ServicesResponseDto> CreateAsync(CreateServicesRequestDto dto) { var st = await _unitOfWork.ServiceTypes.GetByIdAsync(dto.ServiceTypeId); if (st == null) throw new Exception("ServiceType not found"); var e = _mapper.Map<Domain.Entities.Services>(dto); e.CreatedAt = DateTime.UtcNow; await _unitOfWork.Services.CreateAsync(e); await _unitOfWork.SaveChangesAsync(); return _mapper.Map<ServicesResponseDto>(e); }
        public async Task UpdateAsync(int id, UpdateServicesRequestDto dto)
        {
            var e = await _unitOfWork.Services.GetByIdAsync(id); 
            if (e == null) throw new Exception("Services not found");
             var st = await _unitOfWork.ServiceTypes.GetByIdAsync(dto.ServiceTypeId); 
             if (st == null) throw new Exception("ServiceType not found"); 
             if (dto.IsAvailable == null) dto.IsAvailable = e.IsAvailable; _mapper.Map(dto, e); 
             await _unitOfWork.Services.UpdateAsync(e); 
             await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id) { var e = await _unitOfWork.Services.GetByIdAsync(id); if (e == null) throw new Exception("Services not found"); await _unitOfWork.Services.DeleteAsync(e); await _unitOfWork.SaveChangesAsync(); }
    }
}
