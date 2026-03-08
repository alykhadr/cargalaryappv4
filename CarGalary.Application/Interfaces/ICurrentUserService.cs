

namespace CarGalary.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? UserName { get; }
        string? Email { get; }
        int? BranchId { get; }
        bool IsInRole(string roleName);
    }
}
