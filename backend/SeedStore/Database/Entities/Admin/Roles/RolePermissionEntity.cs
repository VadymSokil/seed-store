namespace SeedStore.Database.Entities.Admin.Roles
{
    public class RolePermissionEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public int? ViewOrder { get; set; }
    }
}
