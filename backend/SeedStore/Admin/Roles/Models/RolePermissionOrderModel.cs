namespace SeedStore.Admin.Roles.Models
{
    public class RolePermissionOrderModel
    {
        public int PermissionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ViewOrder { get; set; }
    }
}
