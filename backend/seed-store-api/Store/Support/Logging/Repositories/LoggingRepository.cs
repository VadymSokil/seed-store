using seed_store_api.Database.Context;
using seed_store_api.Database.Entities.Store.Support.Logging;
using seed_store_api.Store.Support.Logging.Interfaces;

namespace seed_store_api.Store.Support.Logging.Repositories
{
    public class LoggingRepository : ILoggingRepository
    {
        private readonly AppDbContext _context;

        public LoggingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddLogAsync(LoggingEntity log)
        {
            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}