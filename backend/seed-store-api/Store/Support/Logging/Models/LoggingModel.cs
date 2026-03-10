namespace seed_store_api.Store.Support.Logging.Models
{
    public class LoggingModel
    {
        public string Status { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Input { get; set; }
        public string? Output { get; set; }
        public string? Message { get; set; }
        public long ExecutionTime { get; set; }
    }
}
