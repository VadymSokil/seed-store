namespace SeedStore.Admin.Stats.Models
{
    public class ActiveProcessingResponseModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
    }
}
