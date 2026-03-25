using Microsoft.Extensions.Caching.Memory;
using SeedStore.Admin.Roles.Interfaces;
using SeedStore.Admin.Roles.Models;
using SeedStore.Database.Entities.Admin.Roles;
using SeedStore.Support.Admin.EmployeesActivity.Interfaces;
using SeedStore.Support.Admin.Reorder.Models;
using SeedStore.Support.General.Constants.CacheKeys;
using System.Text.Json;

namespace SeedStore.Admin.Roles.Services
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _repository;
        private readonly IEmployeesActivityService _employeesActivityService;
        private readonly IMemoryCache _cache;

        public RolesService(IRolesRepository repository, IEmployeesActivityService employeesActivityService, IMemoryCache cache)
        {
            _repository = repository;
            _employeesActivityService = employeesActivityService;
            _cache = cache;
        }

        public async Task<List<RolesResponseModel>> GetRolesAsync()
        {
            var roles = await _repository.GetRolesAsync();

            return roles.Select(r => new RolesResponseModel
            {
                Id = r.Id,
                Code = r.Code,
                Name = r.Name,
                Priority = r.Priority,
                IsActive = r.IsActive,
                ViewOrder = r.ViewOrder
            }).ToList();
        }

        public async Task<RoleInfoResponseModel?> GetRoleAsync(int roleId)
        {
            var role = await _repository.GetRoleByIdAsync(roleId);

            if (role == null)
                return null;

            return new RoleInfoResponseModel
            {
                Id = role.Id,
                Code = role.Code,
                Name = role.Name,
                Priority= role.Priority,
                IsActive = role.IsActive,
                ViewOrder = role.ViewOrder
            };
        }

        public async Task<string> AddRoleAsync(ChangeRoleModel model, int initiatorId)
        {
            var codeExists = await _repository.RoleCodeExistsAsync(model.Code);

            if (codeExists)
                return "code_taken";

            await _repository.AddRoleAsync(new RoleEntity
            {
                Code = model.Code,
                Name = model.Name,
                Priority = model.Priority,
                IsActive = model.IsActive,
                ViewOrder = model.ViewOrder
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) роль {model.Name}.");
            return "ok";
        }

        public async Task<string> UpdateRoleAsync(int roleId, ChangeRoleModel model, int initiatorId)
        {
            var role = await _repository.GetRoleByIdAsync(roleId);

            if (role == null)
                return "not_found";

            var codeExists = await _repository.RoleCodeExistsAsync(model.Code);

            if (codeExists && role.Code != model.Code)
                return "code_taken";

            var before = JsonSerializer.Serialize(new { role.Code, role.Name, role.Priority, role.IsActive, role.ViewOrder });

            role.Code = model.Code;
            role.Name = model.Name;
            role.Priority = model.Priority;
            role.IsActive = model.IsActive;
            role.ViewOrder = model.ViewOrder;
            await _repository.UpdateRoleAsync(role);
            var after = JsonSerializer.Serialize(new { model.Code, model.Name, model.Priority, model.IsActive, model.ViewOrder });
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) роль {role.Name}.", before, after);
            return "ok";
        }

        public async Task<string> DeleteRoleAsync(int roleId, int initiatorId)
        {
            var role = await _repository.GetRoleByIdAsync(roleId);

            if (role == null)
                return "not_found";

            await _repository.DeleteRoleAsync(role);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) роль {role.Name}.");
            return "ok";
        }

        public async Task<string> AssignRoleAsync(int employeeId, int roleId, int initiatorId)
        {
            var employee = await _repository.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
                return "not_found";

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            if (employee.Role!.Priority <= initiator!.Role!.Priority)
                return "forbidden";

            var role = await _repository.GetRoleByIdAsync(roleId);
            if (role == null)
                return "role_not_found";

            if (role.Priority <= initiator.Role!.Priority)
                return "forbidden";

            employee.RoleId = roleId;
            await _repository.UpdateEmployeeAsync(employee);
            _cache.Remove(CacheKeys.Employees);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) призначив(ла) роль {role.Name} співробітнику(ці) {employee.Name}({employeeId}).");
            return "ok";
        }

        public async Task<List<ResponsePermissionModel>> GetRolePermissionsAsync(int roleId)
        {
            var cacheKey = $"{CacheKeys.RolePermissions}_{roleId}";

            if (_cache.TryGetValue(cacheKey, out List<ResponsePermissionModel> cached))
                return cached;

            var permissions = await _repository.GetRolePermissionsAsync(roleId);

            var result = permissions.Select(p => new ResponsePermissionModel
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                IsActive = p.IsActive,
                ViewOrder = p.ViewOrder
            }).ToList();

            _cache.Set(cacheKey, result, TimeSpan.FromHours(1));
            return result;
        }

        public async Task<string> SetRolePermissionsAsync(int roleId, List<int> permissionIds, int initiatorId)
        {
            var role = await _repository.GetRoleByIdAsync(roleId);

            if (role == null)
                return "not_found";

            var existing = await _repository.GetRolePermissionEntitiesAsync(roleId);
            await _repository.DeleteRolePermissionsAsync(existing);

            var newPermissions = permissionIds.Select(pid => new RolePermissionEntity
            {
                RoleId = roleId,
                PermissionId = pid
            }).ToList();

            await _repository.AddRolePermissionsAsync(newPermissions);
            _cache.Remove($"{CacheKeys.RolePermissions}_{roleId}");
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) дозволи ролі {role.Name}.");
            return "ok";
        }

        public async Task<List<ResponsePermissionModel>> GetPermissionsAsync()
        {
            if (_cache.TryGetValue(CacheKeys.Permissions, out List<ResponsePermissionModel> cached))
                return cached;

            var permissions = await _repository.GetPermissionsAsync();

            var result = permissions.Select(p => new ResponsePermissionModel
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                IsActive = p.IsActive,
                ViewOrder = p.ViewOrder
            }).ToList();

            _cache.Set(CacheKeys.Permissions, result, TimeSpan.FromHours(1));
            return result;
        }

        public async Task<string> AddPermissionAsync(AddPermissionModel model, int initiatorId)
        {
            var codeExists = await _repository.PermissionCodeExistsAsync(model.Code);

            if (codeExists)
                return "code_taken";

            await _repository.AddPermissionAsync(new PermissionEntity
            {
                Code = model.Code,
                Name = model.Name,
                IsActive = model.IsActive,
                ViewOrder = model.ViewOrder
            });

            _cache.Remove(CacheKeys.Permissions);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) дозвіл {model.Name}.");
            return "ok";
        }

        public async Task<string> UpdatePermissionAsync(int id, UpdatePermissionModel model, int initiatorId)
        {
            var permission = await _repository.GetPermissionByIdAsync(id);

            if (permission == null)
                return "not_found";

            var codeExists = await _repository.PermissionCodeExistsAsync(model.Code);

            if (codeExists && permission.Code != model.Code)
                return "code_taken";

            var before = JsonSerializer.Serialize(new { permission.Code, permission.Name, permission.IsActive, permission.ViewOrder });
            permission.Code = model.Code;
            permission.Name = model.Name;
            permission.IsActive = model.IsActive;
            permission.ViewOrder = model.ViewOrder;
            await _repository.UpdatePermissionAsync(permission);

            _cache.Remove(CacheKeys.Permissions);
            var after = JsonSerializer.Serialize(new { model.Code, model.Name, model.IsActive, model.ViewOrder });
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) дозвіл {permission.Name}.", before, after);
            return "ok";
        }

        public async Task<string> DeletePermissionAsync(int id, int initiatorId)
        {
            var permission = await _repository.GetPermissionByIdAsync(id);

            if (permission == null)
                return "not_found";

            await _repository.DeletePermissionAsync(permission);

            _cache.Remove(CacheKeys.Permissions);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) дозвіл {permission.Name}.");
            return "ok";
        }

        public async Task ReorderRolesAsync(List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetRolesOrderAsync();
            var before = JsonSerializer.Serialize(currentOrder);
            await _repository.ReorderRolesAsync(items);
            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок ролей.", before, after);
        }

        public async Task ReorderPermissionsAsync(List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetPermissionsOrderAsync();
            var before = JsonSerializer.Serialize(currentOrder);
            await _repository.ReorderPermissionsAsync(items);
            _cache.Remove(CacheKeys.Permissions);
            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок дозволів.", before, after);
        }

        public async Task ReorderRolePermissionsAsync(int roleId, List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetRolePermissionsOrderAsync(roleId);
            var before = JsonSerializer.Serialize(currentOrder);
            await _repository.ReorderRolePermissionsAsync(items);
            _cache.Remove($"{CacheKeys.RolePermissions}_{roleId}");
            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var role = await _repository.GetRoleByIdAsync(roleId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок дозволів ролі ({role!.Name}).", before, after);
        }
    }
}