using SeedStore.Admin.Roles.Models;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Admin.Roles;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Roles.Interfaces
{
    public interface IRolesRepository
    {
        Task<List<RoleEntity>> GetRolesAsync();
        Task<RoleEntity?> GetRoleByIdAsync(int roleId);
        Task<bool> RoleCodeExistsAsync(string code);
        Task AddRoleAsync(RoleEntity role);
        Task UpdateRoleAsync(RoleEntity role);
        Task DeleteRoleAsync(RoleEntity role);
        Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId);
        Task UpdateEmployeeAsync(EmployeeEntity employee);

        Task<List<PermissionEntity>> GetRolePermissionsAsync(int roleId);
        Task<List<RolePermissionEntity>> GetRolePermissionEntitiesAsync(int roleId);
        Task DeleteRolePermissionsAsync(List<RolePermissionEntity> permissions);
        Task AddRolePermissionsAsync(List<RolePermissionEntity> permissions);
        Task<List<PermissionEntity>> GetPermissionsAsync();
        Task<PermissionEntity?> GetPermissionByIdAsync(int id);
        Task<bool> PermissionCodeExistsAsync(string code);
        Task AddPermissionAsync(PermissionEntity entity);
        Task UpdatePermissionAsync(PermissionEntity entity);
        Task DeletePermissionAsync(PermissionEntity entity);

        Task ReorderRolesAsync(List<ReorderItemModel> items);
        Task ReorderPermissionsAsync(List<ReorderItemModel> items);
        Task ReorderRolePermissionsAsync(List<ReorderItemModel> items);

        Task<List<ReorderItemModel>> GetRolesOrderAsync();
        Task<List<ReorderItemModel>> GetPermissionsOrderAsync();
        Task<List<RolePermissionOrderModel>> GetRolePermissionsOrderAsync(int roleId);
    }
}