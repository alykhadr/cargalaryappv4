using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateEmployeeAsync(RegisterRequest request, Guid userId)
        {
            await ValidateCreateRequestAsync(request);
            var employeeNo = string.IsNullOrWhiteSpace(request.EmployeeNo)
                ? $"EMP-{Guid.NewGuid():N}".ToUpperInvariant()
                : request.EmployeeNo.Trim();

            var employee = new Employee
            {
                UserId = userId,
                BranchId = request.BranchId,
                EmployeeNo = employeeNo,
                NationalId = request.NationalId!.Trim(),
                JobTitle = request.JobTitle?.Trim() ?? string.Empty,
                DepartmentId = request.DepartmentId,
                HireDate = request.HireDate ?? DateTime.UtcNow,
                TerminationDate = request.TerminationDate,
                EmploymentStatus = string.IsNullOrWhiteSpace(request.EmploymentStatus) ? "Active" : request.EmploymentStatus.Trim(),
                WorkEmail = request.WorkEmail?.Trim(),
                WorkPhone = request.WorkPhone?.Trim(),
                Extension = request.Extension?.Trim(),
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender?.Trim(),
                Nationality = request.Nationality?.Trim(),
                AddressLine1 = request.AddressLine1?.Trim(),
                AddressLine2 = request.AddressLine2?.Trim(),
                City = request.City?.Trim(),
                Region = request.Region?.Trim(),
                PostalCode = request.PostalCode?.Trim()
            };

            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<UserListItemDto>> GetEmployeesAsync()
        {
            var employees = await _unitOfWork.Employees.GetAllWithDetailsAsync();
            return employees.Select(MapToListItem).ToList();
        }

        public async Task<List<UserListItemDto>> GetEmployeesByBranchAsync(int branchId)
        {
            var employees = await _unitOfWork.Employees.GetByBranchIdWithDetailsAsync(branchId);
            return employees.Select(MapToListItem).ToList();
        }

        public async Task<List<UserListItemDto>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            var employees = await _unitOfWork.Employees.GetByDepartmentIdWithDetailsAsync(departmentId);
            return employees.Select(MapToListItem).ToList();
        }

        public async Task UpdateEmployeeAsync(Guid userId, UpdateAdminUserRequest request)
        {
            var employee = await _unitOfWork.Employees.GetByUserIdAsync(userId);
            if (employee == null)
            {
                throw new Exception("Employee not found");
            }

            if (request.DepartmentId <= 0)
            {
                throw new Exception("Department is required");
            }

            var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
            if (department == null)
            {
                throw new Exception("Department not found");
            }

            employee.BranchId = request.BranchId;
            employee.DepartmentId = request.DepartmentId;
            if (!string.IsNullOrWhiteSpace(request.EmployeeNo)) employee.EmployeeNo = request.EmployeeNo.Trim();
            if (!string.IsNullOrWhiteSpace(request.NationalId)) employee.NationalId = request.NationalId.Trim();
            if (!string.IsNullOrWhiteSpace(request.JobTitle)) employee.JobTitle = request.JobTitle.Trim();
            if (request.HireDate.HasValue) employee.HireDate = request.HireDate.Value;
            employee.TerminationDate = request.TerminationDate;
            if (!string.IsNullOrWhiteSpace(request.EmploymentStatus)) employee.EmploymentStatus = request.EmploymentStatus.Trim();
            employee.WorkEmail = request.WorkEmail?.Trim();
            employee.WorkPhone = request.WorkPhone?.Trim();
            employee.Extension = request.Extension?.Trim();
            employee.DateOfBirth = request.DateOfBirth;
            employee.Gender = request.Gender?.Trim();
            employee.Nationality = request.Nationality?.Trim();
            employee.AddressLine1 = request.AddressLine1?.Trim();
            employee.AddressLine2 = request.AddressLine2?.Trim();
            employee.City = request.City?.Trim();
            employee.Region = request.Region?.Trim();
            employee.PostalCode = request.PostalCode?.Trim();
            employee.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(Guid userId)
        {
            var employee = await _unitOfWork.Employees.GetByUserIdAsync(userId);
            if (employee == null)
            {
                return;
            }

            await _unitOfWork.Employees.DeleteAsync(employee);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task ValidateCreateRequestAsync(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NationalId))
            {
                throw new Exception("National ID is required");
            }

            if (request.DepartmentId <= 0)
            {
                throw new Exception("Department is required");
            }

            var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
            if (department == null)
            {
                throw new Exception("Department not found");
            }
        }

        private static UserListItemDto MapToListItem(Employee employee)
        {
            return new UserListItemDto
            {
                Id = employee.UserId.ToString(),
                EmployeeId = employee.Id,
                UserName = employee.User?.UserName ?? string.Empty,
                Email = employee.User?.Email ?? string.Empty,
                MobileNo = employee.User?.PhoneNumber,
                BranchName = employee.Branch?.BranchNameEn ?? string.Empty,
                FirstName = employee.User?.FullNameEn ?? string.Empty,
                LastName = employee.User?.FullNameAr ?? string.Empty,
                IsLocked = employee.User?.LockoutEnd.HasValue == true && employee.User.LockoutEnd.Value > DateTimeOffset.UtcNow,
                CreatedAt = employee.CreatedAt,
                BranchId = employee.BranchId,
                ProfileImageUrl = employee.User?.ProfileImageUrl,
                EmployeeNo = employee.EmployeeNo,
                NationalId = employee.NationalId,
                JobTitle = employee.JobTitle,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.NameEn ?? employee.Department?.NameAr ?? string.Empty,
                HireDate = employee.HireDate,
                TerminationDate = employee.TerminationDate,
                EmploymentStatus = employee.EmploymentStatus,
                WorkEmail = employee.WorkEmail,
                WorkPhone = employee.WorkPhone,
                Extension = employee.Extension,
                DateOfBirth = employee.DateOfBirth,
                Gender = employee.Gender,
                Nationality = employee.Nationality,
                AddressLine1 = employee.AddressLine1,
                AddressLine2 = employee.AddressLine2,
                City = employee.City,
                Region = employee.Region,
                PostalCode = employee.PostalCode
            };
        }
    }
}
