using SeedStore.Database.Entities.Admin.Account;

namespace SeedStore.Support.Admin.EmployeesActivity.Interfaces
{
    public interface IEmployeesActivityRepository
    {
        Task AddActivityAsync(EmployeeActivityEntity activity);
    }
}
