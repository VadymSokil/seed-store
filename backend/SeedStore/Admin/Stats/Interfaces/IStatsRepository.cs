using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Store.Orders;
using SeedStore.Database.Entities.Store.Reviews;

namespace SeedStore.Admin.Stats.Interfaces
{
    public interface IStatsRepository
    {
        Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId);
        Task UpdateEmployeeAsync(EmployeeEntity employee);
        Task<List<EmployeeEntity>> GetActiveEmployeesAsync();
        Task<List<EmployeeActivityEntity>> GetActivityAsync(int? employeeId, DateTime? dateFrom, DateTime? dateTo, string? search);
        Task<List<(string StatusCode, int Count)>> GetOrdersStatsAsync(DateTime dateFrom, DateTime dateTo);
        Task<decimal> GetRevenueAsync(DateTime dateFrom, DateTime dateTo);
        Task<List<OrderEntity>> GetOrdersInProcessingAsync();
        Task<List<ReviewEntity>> GetReviewsInProcessingAsync();
    }
}
