namespace SeedStore.Database.Entities.Admin.Orders
{
    public class OrderProcessingEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public string ActionCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
