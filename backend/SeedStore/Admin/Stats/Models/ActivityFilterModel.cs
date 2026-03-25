namespace SeedStore.Admin.Stats.Models
{
    public class ActivityFilterModel
    {
        public int? EmployeeId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? Search { get; set; }
    }
}
