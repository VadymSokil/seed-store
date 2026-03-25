using Microsoft.EntityFrameworkCore;
using SeedStore.Admin.Account.Interfaces;
using SeedStore.Database.Context;
using SeedStore.Database.Entities.Admin.Account;

namespace SeedStore.Admin.Account.Repositories
{
    public class AdminAccountRepository : IAdminAccountRepository
    {
        private readonly AppDbContext _context;

        public AdminAccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeEntity>> GetEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Role)
                .OrderBy(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> LoginExistsAsync(string login)
        {
            return await _context.Employees.AnyAsync(e => e.Login == login);
        }

        public async Task AddEmployeeAsync(EmployeeEntity employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
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

        public async Task DeleteEmployeeAsync(EmployeeEntity employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeRefreshTokensAsync(int employeeId)
        {
            await _context.EmployeeRefreshTokens
                .Where(t => t.EmployeeId == employeeId)
                .ExecuteDeleteAsync();
        }

        public async Task AddBlockAsync(BlockedEmployeeEntity block)
        {
            await _context.BlockedEmployees.AddAsync(block);
            await _context.SaveChangesAsync();
        }

        public async Task<BlockedEmployeeEntity?> GetBlockAsync(int employeeId)
        {
            return await _context.BlockedEmployees
                .FirstOrDefaultAsync(b => b.TargetEmployeeId == employeeId);
        }

        public async Task DeleteBlockAsync(BlockedEmployeeEntity block)
        {
            _context.BlockedEmployees.Remove(block);
            await _context.SaveChangesAsync();
        }
    }
}