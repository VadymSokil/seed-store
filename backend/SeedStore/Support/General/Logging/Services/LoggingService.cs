using SeedStore.Database.Entities.Support.Logging;
using SeedStore.Support.General.Logging.Interfaces;
using SeedStore.Support.General.Logging.Models;
using SeedStore.Support.General.TokenGeneration.Interfaces;
using System.Text.Json;

namespace SeedStore.Support.General.Logging.Services
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