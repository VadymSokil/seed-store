using seed_store_api.Store.Support.Logging.Models;

namespace seed_store_api.Store.Support.Logging.Interfaces
{
    public interface ILoggingService
    {
        Task LogAsync(LoggingModel model);
    }
}
