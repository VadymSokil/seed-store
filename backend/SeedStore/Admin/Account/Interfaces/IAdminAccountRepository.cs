using SeedStore.Database.Entities.Admin.Account;

namespace SeedStore.Admin.Account.Interfaces
{
    public interface IAdminAccountRepository
    {
        Task<List<EmployeeEntity>> GetEmployeesAsync();
        Task<bool> LoginExistsAsync(string login);
        Task AddEmployeeAsync(EmployeeEntity employee);
        Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId);
        Task UpdateEmployeeAsync(EmployeeEntity employee);
        Task DeleteEmployeeAsync(EmployeeEntity employee);
        Task DeleteEmployeeRefreshTokensAsync(int employeeId);
        Task AddBlockAsync(BlockedEmployeeEntity block);
        Task<BlockedEmployeeEntity?> GetBlockAsync(int employeeId);
        Task DeleteBlockAsync(BlockedEmployeeEntity block);
    }
}
