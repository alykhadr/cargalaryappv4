
using CarGalary.Application.Dtos.Branch;
using CarGalary.Application.Dtos.Branch.Command;




namespace CarGalary.Application.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchResponseDto>> GetAllAsync();
        // Task<Branch> GetByIdAsync(int id);
         Task<BranchResponseDto> CreateAsync(CreateBrancRequestDto  createBrancRequestDto);
        // Task<bool> UpdateAsync(Branch branch);
        Task<bool> ActiveAsync(UpdateBranchWorkingDayRequestDto updateBranchWorkingDayRequestDto);


        // // for working days

        // Task<IEnumerable<BranchWorkingDays>> GetWorkingDaysAsync(int branchId);
        // Task<BranchWorkingDays> AddWorkingDayAsync(int branchId, BranchWorkingDays workingDay);
        // Task<bool> UpdateWorkingDayAsync(BranchWorkingDays workingDay);
        // Task<bool> DeleteWorkingDayAsync(int workingDayId);
    }
}