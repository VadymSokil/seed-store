using SeedStore.Admin.Roles.Models;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Roles.Interfaces
{
    public interface IRolesService
    {
        Task<List<RolesResponseModel>> GetRolesAsync();
        Task<RoleInfoResponseModel?> GetRoleAsync(int roleId);
        Task<string> AddRoleAsync(ChangeRoleModel model, int initiatorId);
        Task<string> UpdateRoleAsync(int roleId, ChangeRoleModel model, int initiatorId);
        Task<string> DeleteRoleAsync(int roleId, int initiatorId);
        Task<string> AssignRoleAsync(int employeeId, int roleId, int initiatorId);

        Task<List<ResponsePermissionModel>> GetRolePermissionsAsync(int roleId);
        Task<string> SetRolePermissionsAsync(int roleId, List<int> permissionIds, int initiatorId);
        Task<List<ResponsePermissionModel>> GetPermissionsAsync();
        Task<string> AddPermissionAsync(AddPermissionModel model, int initiatorId);
        Task<string> UpdatePermissionAsync(int id, UpdatePermissionModel model, int initiatorId);
        Task<string> DeletePermissionAsync(int id, int initiatorId);

        Task ReorderRolesAsync(List<ReorderItemModel> items, int initiatorId);
        Task ReorderPermissionsAsync(List<ReorderItemModel> items, int initiatorId);
        Task ReorderRolePermissionsAsync(int roleId, List<ReorderItemModel> items, int initiatorId);
    }
}