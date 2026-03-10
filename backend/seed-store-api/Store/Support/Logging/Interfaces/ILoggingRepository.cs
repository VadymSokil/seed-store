using seed_store_api.Database.Entities.Store.Support.Logging;

namespace seed_store_api.Store.Support.Logging.Interfaces
{
    public interface ILoggingRepository
    {
        Task AddLogAsync(LoggingEntity log);
    }
}
