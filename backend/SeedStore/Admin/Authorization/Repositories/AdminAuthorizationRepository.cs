using Microsoft.EntityFrameworkCore;
using SeedStore.Admin.Authorization.Interfaces;
using SeedStore.Database.Context;
using SeedStore.Database.Entities.Admin.Account;

namespace SeedStore.Admin.Authorization.Repositories
{
    public class AdminAuthorizationRepository : IAdminAuthorizationRepository
    {
        private readonly AppDbContext _context;

        public AdminAuthorizationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeEntity?> GetEmployeeByLoginAsync(string login)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Login == login);
        }

        public async Task<EmployeeRefreshTokenEntity?> GetRefreshTokenAsync(string token)
        {
            return await _context.EmployeeRefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task AddRefreshTokenAsync(EmployeeRefreshTokenEntity token)
        {
            await _context.EmployeeRefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRefreshTokenAsync(EmployeeRefreshTokenEntity token)
        {
            _context.EmployeeRefreshTokens.Remove(token);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllRefreshTokensAsync(int employeeId)
        {
            await _context.EmployeeRefreshTokens
                .Where(t => t.EmployeeId == employeeId)
                .ExecuteDeleteAsync();
        }

        public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}