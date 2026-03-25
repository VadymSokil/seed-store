using SeedStore.Database.Entities.Support.Logging;

namespace SeedStore.Support.General.Logging.Interfaces
{
    public interface ILoggingRepository
    {
        Task AddLogAsync(LoggingEntity log);
    }
}
