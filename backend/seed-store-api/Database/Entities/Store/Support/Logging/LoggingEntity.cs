namespace seed_store_api.Database.Entities.Store.Support.Logging
{
    public class LoggingEntity
    {
        public int Id { get; set; }
        public Guid LogId { get; set; }
        public DateTime LogTime { get; set; }
        public string Status {  get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Input {  get; set; } 
        public string? Output { get; set; } 
        public string? Message { get; set; }
        public long ExecutionTime { get; set; }
    }
}
