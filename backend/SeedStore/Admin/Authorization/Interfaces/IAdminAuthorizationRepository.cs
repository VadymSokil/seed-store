using SeedStore.Database.Entities.Admin.Account;

namespace SeedStore.Admin.Authorization.Interfaces
{
    public interface IAdminAuthorizationRepository
    {
        Task<EmployeeEntity?> GetEmployeeByLoginAsync(string login);
        Task<EmployeeRefreshTokenEntity?> GetRefreshTokenAsync(string token);
        Task AddRefreshTokenAsync(EmployeeRefreshTokenEntity token);
        Task DeleteRefreshTokenAsync(EmployeeRefreshTokenEntity token);
        Task DeleteAllRefreshTokensAsync(int employeeId);
        Task<EmployeeEntity?> GetEmployeeByIdAsync(int id);
    }
}
