using SeedStore.Admin.Stats.Interfaces;
using SeedStore.Admin.Stats.Models;
using SeedStore.Support.Admin.EmployeesActivity.Interfaces;
using SeedStore.Support.Store.Notifications.Interfaces;

namespace SeedStore.Admin.Stats.Services
{
    public class StatsService : IStatsService
    {
        private readonly IStatsRepository _repository;
        private readonly IEmployeesActivityService _employeesActivityService;
        private readonly INotificationService _notificationService;

        public StatsService(IStatsRepository repository, IEmployeesActivityService employeesActivityService, INotificationService notificationService)
        {
            _repository = repository;
            _employeesActivityService = employeesActivityService;
            _notificationService = notificationService;
        }

        public async Task<string> StartShiftAsync(int employeeId)
        {
            var employee = await _repository.GetEmployeeByIdAsync(employeeId);

            if (employee == null)
                return "not_found";

            if (employee.IsOnShift)
                return "already_on_shift";

            employee.IsOnShift = true;
            await _repository.UpdateEmployeeAsync(employee);
            await _notificationService.SendEmployeeOnShiftAsync();

            await _employeesActivityService.LogAsync(employeeId, $"{employee.Role!.Name} {employee.Name}({employeeId}) заступив(ла) на зміну.");

            return "ok";
        }

        public async Task<string> EndShiftAsync(int employeeId)
        {
            var employee = await _repository.GetEmployeeByIdAsync(employeeId);

            if (employee == null)
                return "not_found";

            if (!employee.IsOnShift)
                return "not_on_shift";

            employee.IsOnShift = false;
            await _repository.UpdateEmployeeAsync(employee);
            await _notificationService.SendEmployeeOffShiftAsync();

            await _employeesActivityService.LogAsync(employeeId, $"{employee.Role!.Name} {employee.Name}({employeeId}) покинув(ла) зміну.");

            return "ok";
        }

        public async Task<List<ActiveEmployeesModel>> GetActiveEmployeesAsync()
        {
            var employees = await _repository.GetActiveEmployeesAsync();

            return employees.Select(e => new ActiveEmployeesModel
            {
                Id = e.Id,
                Name = e.Name,
                RoleName = e.Role!.Name
            }).ToList();
        }

        public async Task<List<ResponseActivityModel>> GetActivityAsync(ActivityFilterModel filter)
        {
            var activity = await _repository.GetActivityAsync(
                filter.EmployeeId,
                filter.DateFrom,
                filter.DateTo,
                filter.Search);

            return activity.Select(a => new ResponseActivityModel
            {
                Id = a.Id,
                EmployeeId = a.EmployeeId,
                Action = a.Action,
                CreatedAt = a.CreatedAt
            }).ToList();
        }

        public async Task<List<OrdersStatsModel>> GetOrdersStatsAsync(DateTime dateFrom, DateTime dateTo)
        {
            var stats = await _repository.GetOrdersStatsAsync(dateFrom, dateTo);

            return stats.Select(s => new OrdersStatsModel
            {
                StatusCode = s.StatusCode,
                Count = s.Count,
                DateFrom = dateFrom,
                DateTo = dateTo
            }).ToList();
        }

        public async Task<RevenueStatsModel> GetRevenueStatsAsync(DateTime dateFrom, DateTime dateTo)
        {
            var revenue = await _repository.GetRevenueAsync(dateFrom, dateTo);

            return new RevenueStatsModel
            {
                TotalRevenue = revenue,
                DateFrom = dateFrom,
                DateTo = dateTo
            };
        }

        public async Task<List<ActiveProcessingResponseModel>> GetActiveProcessingAsync()
        {
            var orders = await _repository.GetOrdersInProcessingAsync();
            var reviews = await _repository.GetReviewsInProcessingAsync();

            var result = new List<ActiveProcessingResponseModel>();

            result.AddRange(orders.Select(o => new ActiveProcessingResponseModel
            {
                EmployeeId = o.TakenByEmployeeId!.Value,
                EmployeeName = o.TakenByEmployee!.Name,
                RoleName = o.TakenByEmployee.Role!.Name,
                Action = $"Обробляє замовлення {o.OrderNumber}"
            }));

            result.AddRange(reviews.Select(r => new ActiveProcessingResponseModel
            {
                EmployeeId = r.TakenByEmployeeId!.Value,
                EmployeeName = r.TakenByEmployee!.Name,
                RoleName = r.TakenByEmployee.Role!.Name,
                Action = $"Модерує відгук покупця {r.Account!.FirstName} {r.Account.LastName[0]} на товар \"{r.ProductNameSnapshot}\""
            }));

            return result;
        }
    }
}