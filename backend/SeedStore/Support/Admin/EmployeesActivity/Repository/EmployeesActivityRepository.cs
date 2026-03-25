using SeedStore.Database.Context;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Support.Admin.EmployeesActivity.Interfaces;

namespace SeedStore.Support.Admin.EmployeesActivity.Repository
{
    public class EmployeesActivityRepository : IEmployeesActivityRepository
    {
        private readonly AppDbContext _context;

        public EmployeesActivityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddActivityAsync(EmployeeActivityEntity activity)
        {
            await _context.EmployeesActivity.AddAsync(activity);
            await _context.SaveChangesAsync();
        }
    }
}