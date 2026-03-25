using Microsoft.Extensions.Caching.Memory;
using SeedStore.Admin.Account.Interfaces;
using SeedStore.Admin.Account.Models;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Support.Admin.EmployeesActivity.Interfaces;
using SeedStore.Support.General.Constants.CacheKeys;
using SeedStore.Support.General.PasswordHash.Interfaces;

namespace SeedStore.Admin.Account.Services
{
    public class AdminAccountService : IAdminAccountService
    {
        private readonly IAdminAccountRepository _repository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IEmployeesActivityService _employeesActivityService;
        private readonly IMemoryCache _cache;

        public AdminAccountService(IAdminAccountRepository repository, IPasswordHashService passwordHashService, IEmployeesActivityService employeesActivityService, IMemoryCache cache)
        {
            _repository = repository;
            _passwordHashService = passwordHashService;
            _employeesActivityService = employeesActivityService;
            _cache = cache;
        }

        public async Task<List<EmployeesResponseModel>> GetEmployeesAsync()
        {
            if (_cache.TryGetValue(CacheKeys.Employees, out List<EmployeesResponseModel> cached))
                return cached;

            var employees = await _repository.GetEmployeesAsync();

            var result = employees.Select(e => new EmployeesResponseModel
            {
                Id = e.Id,
                Name = e.Name,
                Login = e.Login,
                RoleId = e.RoleId,
                RoleName = e.Role!.Name,
                IsBlocked = e.IsBlocked,
                CreatedAt = e.CreatedAt
            }).ToList();

            _cache.Set(CacheKeys.Employees, result, TimeSpan.FromHours(1));
            return result;
        }

        public async Task<string> AddEmployeeAsync(AddEmployeeModel model, int initiatorId)
        {
            var loginExists = await _repository.LoginExistsAsync(model.Login);

            if (loginExists)
                return "login_taken";

            var passwordHash = _passwordHashService.Hash(model.Password);

            await _repository.AddEmployeeAsync(new EmployeeEntity
            {
                Name = model.Name,
                Login = model.Login,
                PasswordHash = passwordHash,
                RoleId = model.RoleId,
                IsBlocked = false,
                CreatedAt = DateTime.UtcNow
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) створив(ла) акаунт для {model.Name}.");

            _cache.Remove(CacheKeys.Employees);
            return "ok";
        }

        public async Task<string> ChangeEmployeeNameAsync(int employeeId, string name, int initiatorId)
        {
            var employee = await _repository.GetEmployeeByIdAsync(employeeId);

            if (employee == null)
                return "not_found";

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            if (employee.Role!.Priority <= initiator!.Role!.Priority)
                return "forbidden";

            var before = employee.Name;

            employee.Name = name;
            await _repository.UpdateEmployeeAsync(employee);

            var after = name;
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) ім'я співробітника {employee.Name}({employee.Id}).", before, after);

            _cache.Remove(CacheKeys.Employees);
            return "ok";
        }

        public async Task<string> ChangeEmployeePasswordAsync(int employeeId, string password, int initiatorId)
        {
            var employee = await _repository.GetEmployeeByIdAsync(employeeId);

            if (employee == null)
                return "not_found";

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            if (employee.Role!.Priority <= initiator!.Role!.Priority)
                return "forbidden";

            employee.PasswordHash = _passwordHashService.Hash(password);
            await _repository.UpdateEmployeeAsync(employee);
            await _repository.DeleteEmployeeRefreshTokensAsync(employeeId);

            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) пароль співробітника {employee.Name}({employee.Id}).");

            _cache.Remove(CacheKeys.Employees);
            return "ok";
        }

        public async Task<string> DeleteEmployeeAsync(int employeeId, int initiatorId)
        {
            var employee = await _repository.GetEmployeeByIdAsync(employeeId);

            if (employee == null)
                return "not_found";

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            if (employee.Role!.Priority <= initiator!.Role!.Priority)
                return "forbidden";

            await _repository.DeleteEmployeeRefreshTokensAsync(employeeId);
            await _repository.DeleteEmployeeAsync(employee);

            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) акаунт {employee.Name}({employeeId}).");

            _cache.Remove(CacheKeys.Employees);
            return "ok";
        }

        public async Task<string> BlockEmployeeAsync(int targetEmployeeId, int blockedByEmployeeId, string reason)
        {
            var employee = await _repository.GetEmployeeByIdAsync(targetEmployeeId);

            if (employee == null)
                return "not_found";

            var initiator = await _repository.GetEmployeeByIdAsync(blockedByEmployeeId);
            if (employee.Role!.Priority <= initiator!.Role!.Priority)
                return "forbidden";

            if (employee.IsBlocked)
                return "already_blocked";

            employee.IsBlocked = true;
            await _repository.UpdateEmployeeAsync(employee);

            await _repository.AddBlockAsync(new BlockedEmployeeEntity
            {
                TargetEmployeeId = targetEmployeeId,
                BlockedByEmployeeId = blockedByEmployeeId,
                Reason = reason,
                CreatedAt = DateTime.UtcNow
            });

            await _repository.DeleteEmployeeRefreshTokensAsync(targetEmployeeId);
            await _employeesActivityService.LogAsync(blockedByEmployeeId, $"{initiator!.Role!.Name} {initiator.Name}({blockedByEmployeeId}) заблокував(ла) співробітника {employee.Name}({employee.Id}).", null, reason);

            _cache.Remove(CacheKeys.Employees);
            return "ok";
        }

        public async Task<string> UnblockEmployeeAsync(int employeeId, int initiatorId)
        {
            var employee = await _repository.GetEmployeeByIdAsync(employeeId);

            if (employee == null)
                return "not_found";

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            if (employee.Role!.Priority <= initiator!.Role!.Priority)
                return "forbidden";

            if (!employee.IsBlocked)
                return "not_blocked";

            employee.IsBlocked = false;
            await _repository.UpdateEmployeeAsync(employee);

            var block = await _repository.GetBlockAsync(employeeId);

            if (block != null)
                await _repository.DeleteBlockAsync(block);

            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) розблокував(ла) співробітника {employee.Name}({employee.Id}).");

            _cache.Remove(CacheKeys.Employees);
            return "ok";
        }
    }
}