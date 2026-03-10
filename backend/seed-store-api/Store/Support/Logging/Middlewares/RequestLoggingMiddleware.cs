using seed_store_api.Store.Support.Logging.Interfaces;
using seed_store_api.Store.Support.Logging.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace seed_store_api.Store.Support.Logging.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            context.Request.EnableBuffering();
            var input = await ReadRequestBodyAsync(context.Request);
            var originalResponseBody = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;
            var stopwatch = Stopwatch.StartNew();

            Exception? caughtException = null;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            stopwatch.Stop();
            var output = await ReadResponseBodyAsync(context.Response);
            context.Response.Body = originalResponseBody;
            await responseBodyStream.CopyToAsync(originalResponseBody);

            var statusCode = caughtException != null ? 500 : context.Response.StatusCode;

            string? message = null;
            if ((statusCode == 400 || statusCode == 409) && !string.IsNullOrEmpty(output))
            {
                try
                {
                    var doc = JsonDocument.Parse(output);
                    if (doc.RootElement.TryGetProperty("title", out var title))
                        message = title.GetString();
                }
                catch
                {
                    message = output;
                }
            }

            using var scope = _scopeFactory.CreateScope();
            var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
            await loggingService.LogAsync(new LoggingModel
            {
                Status = statusCode.ToString(),
                Address = $"endpoint:{context.Request.Method}:{context.Request.Path}",
                Input = string.IsNullOrEmpty(input) ? null : input,
                Output = string.IsNullOrEmpty(output) ? null : output,
                Message = message,
                ExecutionTime = stopwatch.ElapsedMilliseconds
            });

            Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] REQUEST {context.Request.Method} {context.Request.Path} | Status: {statusCode} | Time: {stopwatch.ElapsedMilliseconds}ms");

            if (caughtException != null)
                throw caughtException;
        }

        private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }

        private static async Task<string> ReadResponseBodyAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(response.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return body;
        }
    }
}