namespace SeedStore.Admin.Stats.Models
{
    public class OrdersStatsModel
    {
        public string StatusCode { get; set; } = string.Empty;
        public int Count { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

    }
}
