using SeedStore.Admin.Account.Models;

namespace SeedStore.Admin.Account.Interfaces
{
    public interface IAdminAccountService
    {
        Task<List<EmployeesResponseModel>> GetEmployeesAsync();
        Task<string> AddEmployeeAsync(AddEmployeeModel model, int initiatorId);
        Task<string> ChangeEmployeeNameAsync(int employeeId, string name, int initiatorId);
        Task<string> ChangeEmployeePasswordAsync(int employeeId, string password, int initiatorId);
        Task<string> DeleteEmployeeAsync(int employeeId, int initiatorId);
        Task<string> BlockEmployeeAsync(int targetEmployeeId, int blockedByEmployeeId, string reason);
        Task<string> UnblockEmployeeAsync(int employeeId, int initiatorId);
    }
}
