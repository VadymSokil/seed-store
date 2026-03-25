namespace SeedStore.Support.Admin.EmployeesActivity.Interfaces
{
    public interface IEmployeesActivityService
    {
        Task LogAsync(int employeeId, string action, string? before = null, string? after = null);
    }
}
