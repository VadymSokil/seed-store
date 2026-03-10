using seed_store_api.Store.Support.Logging.Interfaces;
using seed_store_api.Store.Support.Logging.Models;
using System.Diagnostics;
using System.Net;

namespace seed_store_api.Store.Support.ExceptionHandling.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                await HandleExceptionAsync(context, ex, stopwatch.ElapsedMilliseconds);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, long executionTime)
        {
            Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] EXCEPTION {context.Request.Path} | Message: {ex.Message}\n{ex.InnerException?.Message}");

            using var scope = _scopeFactory.CreateScope();
            var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();

            await loggingService.LogAsync(new LoggingModel
            {
                Status = "error",
                Address = $"system:error:{context.Request.Path}",
                Message = ex.Message,
                ExecutionTime = executionTime
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(string.Empty);
        }
    }
}