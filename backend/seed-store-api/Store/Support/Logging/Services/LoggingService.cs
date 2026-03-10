using seed_store_api.Database.Entities.Store.Support.Logging;
using seed_store_api.Store.Support.Logging.Interfaces;
using seed_store_api.Store.Support.Logging.Models;
using seed_store_api.Store.Support.TokenGeneration.Interfaces;
using System.Text.Json;

namespace seed_store_api.Store.Support.Logging.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ILoggingRepository _loggingRepository;
        private readonly ITokenGenerationService _tokenGenerationService;

        public LoggingService(ILoggingRepository loggingRepository, ITokenGenerationService tokenGenerationService)
        {
            _loggingRepository = loggingRepository;
            _tokenGenerationService = tokenGenerationService;
        }

        public async Task LogAsync(LoggingModel model)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var log = new LoggingEntity
                {
                    LogId = _tokenGenerationService.GenerateLogId(),
                    LogTime = DateTime.UtcNow,
                    Status = model.Status,
                    Address = model.Address,
                    Input = model.Input,
                    Output = model.Output,
                    Message = model.Message,
                    ExecutionTime = model.ExecutionTime
                };

                await _loggingRepository.AddLogAsync(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] LOGGING ERROR: {ex.Message}\n{ex.InnerException?.Message}");
            }
        }
    }
}