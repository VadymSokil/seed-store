using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Context;

namespace seed_store_api.Store.Support.Cleanup.Services
{
    public class CleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CleanupService(IServiceScopeFactory _scopeFactory)
        {
            this._scopeFactory = _scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await context.PendingAccounts
                    .Where(p => p.ExpiresAt < DateTime.UtcNow)
                    .ExecuteDeleteAsync(stoppingToken);

                await context.PasswordResetRequests
                    .Where(p => p.ExpiresAt < DateTime.UtcNow)
                    .ExecuteDeleteAsync(stoppingToken);

                await context.Products
                    .Where(p => p.DiscountEndDate != null && p.DiscountEndDate < DateTime.UtcNow)
                    .ExecuteUpdateAsync(p => p
                        .SetProperty(x => x.HasDiscount, false)
                        .SetProperty(x => x.DiscountPrice, (decimal?)null)
                        .SetProperty(x => x.DiscountEndDate, (DateTime?)null),
                    stoppingToken);

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}