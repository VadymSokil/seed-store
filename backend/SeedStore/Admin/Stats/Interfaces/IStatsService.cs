using SeedStore.Admin.Stats.Models;

namespace SeedStore.Admin.Stats.Interfaces
{
    public interface IStatsService
    {
        Task<string> StartShiftAsync(int employeeId);
        Task<string> EndShiftAsync(int employeeId);
        Task<List<ActiveEmployeesModel>> GetActiveEmployeesAsync();
        Task<List<ResponseActivityModel>> GetActivityAsync(ActivityFilterModel filter);
        Task<List<OrdersStatsModel>> GetOrdersStatsAsync(DateTime dateFrom, DateTime dateTo);
        Task<RevenueStatsModel> GetRevenueStatsAsync(DateTime dateFrom, DateTime dateTo);
        Task<List<ActiveProcessingResponseModel>> GetActiveProcessingAsync();
    }
}
