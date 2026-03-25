namespace SeedStore.Admin.Orders.Models
{
    public class ResponseOrderActionsModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
