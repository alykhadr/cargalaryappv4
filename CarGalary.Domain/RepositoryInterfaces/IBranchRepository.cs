
using CarGalary.Domain.Entities;


namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IBranchRepository
    {
        Task<IEnumerable<Branchs>> GetAllAsync();
        Task<Branchs> GetByIdAsync(int id);
        Task<Branchs> CreateAsync(Branchs branch);
        Task UpdateAsync(Branchs branch);
        Task  DeleteAsync(Branchs branch);
        Task  DeActiveAsync(Branchs branch);
        Task  ActiveAsync(Branchs branch);


        // for working days

        // Task<IEnumerable<BranchWorkingDays>> GetWorkingDaysAsync(int branchId);
        Task  AddWorkingDayAsync(BranchWorkingDays workingDay);
        Task UpdateWorkingDayAsync(BranchWorkingDays workingDay);
        Task DeleteWorkingDayAsync(BranchWorkingDays workingDay);
    }
}