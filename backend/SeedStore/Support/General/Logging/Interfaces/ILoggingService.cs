using SeedStore.Support.General.Logging.Models;

namespace SeedStore.Support.General.Logging.Interfaces
{
    public interface ILoggingService
    {
        Task LogAsync(LoggingModel model);
    }
}
