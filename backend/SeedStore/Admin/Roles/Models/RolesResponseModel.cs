namespace SeedStore.Admin.Roles.Models
{
    public class RolesResponseModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Priority { get; set; }
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
