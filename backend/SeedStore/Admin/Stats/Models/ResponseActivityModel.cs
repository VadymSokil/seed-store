namespace SeedStore.Admin.Stats.Models
{
    public class ResponseActivityModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

    }
}
