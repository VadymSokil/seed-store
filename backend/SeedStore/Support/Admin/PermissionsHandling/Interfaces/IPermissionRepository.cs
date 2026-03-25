namespace SeedStore.Support.Admin.PermissionsHandling.Interfaces
{
    public interface IPermissionRepository
    {
        Task<List<string>> GetRolePermissionCodesByRoleCodeAsync(string roleCode);
    }
}
