using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Context;
using seed_store_api.Store.Support.Logging.Interfaces;
using seed_store_api.Store.Support.Logging.Models;
using System.Diagnostics;

namespace seed_store_api.Store.Support.Cleanup.Services
{
    public class CleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();

                await RunCleanupAsync(loggingService, "cleaner:pending-accounts", async () =>
                {
                    return await context.PendingAccounts
                        .Where(p => p.ExpiresAt < DateTime.UtcNow)
                        .ExecuteDeleteAsync(stoppingToken);
                });

                await RunCleanupAsync(loggingService, "cleaner:password-reset-requests", async () =>
                {
                    return await context.PasswordResetRequests
                        .Where(p => p.ExpiresAt < DateTime.UtcNow)
                        .ExecuteDeleteAsync(stoppingToken);
                });

                await RunCleanupAsync(loggingService, "cleaner:product-discounts", async () =>
                {
                    return await context.Products
                        .Where(p => p.DiscountEndDate != null && p.DiscountEndDate < DateTime.UtcNow)
                        .ExecuteUpdateAsync(p => p
                            .SetProperty(x => x.HasDiscount, false)
                            .SetProperty(x => x.DiscountPrice, (decimal?)null)
                            .SetProperty(x => x.DiscountEndDate, (DateTime?)null),
                        stoppingToken);
                });

                await RunCleanupAsync(loggingService, "cleaner:email-change-requests", async () =>
                {
                   return await context.EmailChangeRequests
                        .Where(e => e.ExpiresAt < DateTime.UtcNow)
                        .ExecuteDeleteAsync(stoppingToken);
                });

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task RunCleanupAsync(ILoggingService loggingService, string address, Func<Task<int>> action)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var count = await action();
                stopwatch.Stop();
                Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] CLEANER {address} | Deleted: {count} | Time: {stopwatch.ElapsedMilliseconds}ms");
                await loggingService.LogAsync(new LoggingModel
                {
                    Status = "ok",
                    Address = address,
                    ExecutionTime = stopwatch.ElapsedMilliseconds
                });
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] CLEANER ERROR {address} | Message: {ex.Message}\n{ex.InnerException?.Message}");
                await loggingService.LogAsync(new LoggingModel
                {
                    Status = "error",
                    Address = address,
                    Message = ex.Message,
                    ExecutionTime = stopwatch.ElapsedMilliseconds
                });
            }
        }
    }
}