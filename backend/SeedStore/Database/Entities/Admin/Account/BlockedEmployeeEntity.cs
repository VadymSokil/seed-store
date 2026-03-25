namespace SeedStore.Database.Entities.Admin.Account
{
    public class BlockedEmployeeEntity
    {
        public int Id { get; set; }
        public int TargetEmployeeId { get; set; }
        public int BlockedByEmployeeId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
