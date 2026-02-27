using AutoMapper;
using CarGalary.Application.Dtos.Department.Command;
using CarGalary.Application.Dtos.Department.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<DepartmentResponseDto>> GetAllAsync()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            return _mapper.Map<IEnumerable<DepartmentResponseDto>>(departments);
        }

        public async Task<DepartmentResponseDto> GetByIdAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                throw new Exception("Department not found");
            }

            return _mapper.Map<DepartmentResponseDto>(department);
        }

        public async Task<DepartmentResponseDto> CreateAsync(CreateDepartmentRequestDto requestDto)
        {
            var nameAr = requestDto.NameAr?.Trim() ?? string.Empty;
            var nameEn = requestDto.NameEn?.Trim() ?? string.Empty;

            await EnsureUniqueNamesAsync(nameAr, nameEn, null);

            var entity = _mapper.Map<Department>(requestDto);
            entity.NameAr = nameAr;
            entity.NameEn = nameEn;
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;
            entity.IsAvailable = requestDto.IsAvailable ?? true;

            await _unitOfWork.Departments.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DepartmentResponseDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, UpdateDepartmentRequestDto requestDto)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                throw new Exception("Department not found");
            }

            var nameAr = requestDto.NameAr?.Trim() ?? string.Empty;
            var nameEn = requestDto.NameEn?.Trim() ?? string.Empty;

            await EnsureUniqueNamesAsync(nameAr, nameEn, id);

            department.NameAr = nameAr;
            department.NameEn = nameEn;
            department.IsAvailable = requestDto.IsAvailable ?? true;
            department.UpdatedAt = DateTime.UtcNow;
            department.UpdatedBy = _currentUserService.UserName;

            await _unitOfWork.Departments.UpdateAsync(department);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                throw new Exception("Department not found");
            }

            await _unitOfWork.Departments.DeleteAsync(department);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private async Task EnsureUniqueNamesAsync(string nameAr, string nameEn, int? currentId)
        {
            var existingAr = await _unitOfWork.Departments.GetByNameArAsync(nameAr);
            if (existingAr != null && existingAr.Id != currentId)
            {
                throw new Exception("Name (AR) already exists");
            }

            var existingEn = await _unitOfWork.Departments.GetByNameEnAsync(nameEn);
            if (existingEn != null && existingEn.Id != currentId)
            {
                throw new Exception("Name (EN) already exists");
            }
        }
    }
}
