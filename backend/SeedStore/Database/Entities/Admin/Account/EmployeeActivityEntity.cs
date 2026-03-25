namespace SeedStore.Database.Entities.Admin.Account
{
    public class EmployeeActivityEntity
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? Before {  get; set; }
        public string? After { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
