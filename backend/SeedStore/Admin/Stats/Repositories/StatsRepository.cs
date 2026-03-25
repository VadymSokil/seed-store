using Microsoft.EntityFrameworkCore;
using SeedStore.Admin.Stats.Interfaces;
using SeedStore.Database.Context;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Store.Orders;
using SeedStore.Database.Entities.Store.Reviews;

namespace SeedStore.Admin.Stats.Repositories
{
    public class StatsRepository : IStatsRepository
    {
        private readonly AppDbContext _context;

        public StatsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task UpdateEmployeeAsync(EmployeeEntity employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EmployeeEntity>> GetActiveEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Role)
                .Where(e => e.IsOnShift)
                .ToListAsync();
        }

        public async Task<List<EmployeeActivityEntity>> GetActivityAsync(int? employeeId, DateTime? dateFrom, DateTime? dateTo, string? search)
        {
            var query = _context.EmployeesActivity.AsQueryable();

            if (employeeId.HasValue)
                query = query.Where(a => a.EmployeeId == employeeId);

            if (dateFrom.HasValue)
                query = query.Where(a => a.CreatedAt >= dateFrom);

            if (dateTo.HasValue)
                query = query.Where(a => a.CreatedAt <= dateTo);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(a => a.Action.ToLower().Contains(search.ToLower()));

            return await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
        }

        public async Task<List<(string StatusCode, int Count)>> GetOrdersStatsAsync(DateTime dateFrom, DateTime dateTo)
        {
            return await _context.Orders
                .Where(o => o.OrderDate >= dateFrom && o.OrderDate <= dateTo)
                .GroupBy(o => o.StatusCode)
                .Select(g => ValueTuple.Create(g.Key, g.Count()))
                .ToListAsync();
        }

        public async Task<decimal> GetRevenueAsync(DateTime dateFrom, DateTime dateTo)
        {
            return await _context.Orders
                .Where(o => o.OrderDate >= dateFrom && o.OrderDate <= dateTo && o.StatusCode == "completed")
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<List<OrderEntity>> GetOrdersInProcessingAsync()
        {
            return await _context.Orders
                .Include(o => o.TakenByEmployee)
                .Where(o => o.TakenByEmployeeId != null)
                .ToListAsync();
        }

        public async Task<List<ReviewEntity>> GetReviewsInProcessingAsync()
        {
            return await _context.Reviews
                .Include(r => r.TakenByEmployee)
                .Include(r => r.Account)
                .Where(r => r.TakenByEmployeeId != null)
                .ToListAsync();
        }


    }
}