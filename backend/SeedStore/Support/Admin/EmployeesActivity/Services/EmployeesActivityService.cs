using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Support.Admin.EmployeesActivity.Interfaces;

namespace SeedStore.Support.Admin.EmployeesActivity.Services
{
    public class EmployeesActivityService : IEmployeesActivityService
    {
        private readonly IEmployeesActivityRepository _repository;

        public EmployeesActivityService(IEmployeesActivityRepository repository)
        {
            _repository = repository;
        }

        public async Task LogAsync(int employeeId, string action, string? before = null, string? after = null)
        {
            await _repository.AddActivityAsync(new EmployeeActivityEntity
            {
                EmployeeId = employeeId,
                Action = action,
                Before = before,
                After = after,
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}