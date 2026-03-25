using SeedStore.Database.Entities.Admin.Roles;

namespace SeedStore.Database.Entities.Admin.Account
{
    public class EmployeeEntity
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; }
        public RoleEntity? Role { get; set; }
        public bool IsOnShift { get; set; }
    }
}
