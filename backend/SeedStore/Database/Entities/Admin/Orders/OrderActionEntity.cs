namespace SeedStore.Database.Entities.Admin.Orders
{
    public class OrderActionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
