using AutoMapper;
using CarGalary.Application.Dtos.Services.Command;
using CarGalary.Application.Dtos.Services.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;
using Microsoft.AspNetCore.Hosting;

namespace CarGalary.Application.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        
        public ServicesService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment env) 
        { 
            _unitOfWork = unitOfWork; 
            _mapper = mapper;
            _env = env;
        }
        
        public async Task<List<ServicesResponseDto>> GetAllAsync() 
        { 
            var i = await _unitOfWork.Services.GetAllAsync(); 
            return _mapper.Map<List<ServicesResponseDto>>(i); 
        }
        
        public async Task<ServicesResponseDto?> GetByIdAsync(int id) 
        { 
            var i = await _unitOfWork.Services.GetByIdAsync(id); 
            return i == null ? null : _mapper.Map<ServicesResponseDto>(i); 
        }
        
        public async Task<ServicesResponseDto> CreateAsync(CreateServicesRequestDto dto)
        {
            var e = _mapper.Map<Domain.Entities.Services>(dto); 
            e.CreatedAt = DateTime.UtcNow;
            
            if (dto.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "services");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = $"{Guid.NewGuid()}_{dto.ImageFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }
                e.ServiceImageUrl = $"/uploads/services/{fileName}";
            }
            
            await _unitOfWork.Services.CreateAsync(e); 
            await _unitOfWork.SaveChangesAsync(); 
            return _mapper.Map<ServicesResponseDto>(e); 
        }
        
        public async Task UpdateAsync(int id, UpdateServicesRequestDto dto)
        {
            var e = await _unitOfWork.Services.GetByIdAsync(id);
            if (e == null) throw new Exception("Services not found");
            
            if (dto.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(e.ServiceImageUrl))
                {
                    var oldFilePath = Path.Combine(_env.WebRootPath, e.ServiceImageUrl.TrimStart('/'));
                    if (File.Exists(oldFilePath)) File.Delete(oldFilePath);
                }
                
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "services");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = $"{Guid.NewGuid()}_{dto.ImageFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }
                e.ServiceImageUrl = $"/uploads/services/{fileName}";
            }
            
            if (dto.IsAvailable == null) dto.IsAvailable = e.IsAvailable; 
            _mapper.Map(dto, e);
            await _unitOfWork.Services.UpdateAsync(e);
            await _unitOfWork.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(int id) 
        { 
            var e = await _unitOfWork.Services.GetByIdAsync(id); 
            if (e == null) throw new Exception("Services not found"); 
            
            if (!string.IsNullOrEmpty(e.ServiceImageUrl))
            {
                var filePath = Path.Combine(_env.WebRootPath, e.ServiceImageUrl.TrimStart('/'));
                if (File.Exists(filePath)) File.Delete(filePath);
            }
            
            await _unitOfWork.Services.DeleteAsync(e); 
            await _unitOfWork.SaveChangesAsync(); 
        }
        
        public async Task<BulkDeleteServicesResponseDto> BulkDeleteAsync(List<int> serviceIds)
        {
            var response = new BulkDeleteServicesResponseDto();
            foreach (var id in serviceIds)
            {
                try
                {
                    await DeleteAsync(id);
                    response.DeletedCount++;
                }
                catch
                {
                    response.FailedIds.Add(id);
                }
            }
            return response;
        }
    }
}
