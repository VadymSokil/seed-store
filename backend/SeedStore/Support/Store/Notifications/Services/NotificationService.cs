using Microsoft.AspNetCore.SignalR;
using SeedStore.Support.Store.Notifications.Hubs;
using SeedStore.Support.Store.Notifications.Interfaces;

namespace SeedStore.Support.Store.Notifications.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNewOrderAsync() => await _hubContext.Clients.All.SendAsync("NewOrder");
        public async Task SendNewReviewAsync() => await _hubContext.Clients.All.SendAsync("NewReview");
        public async Task SendOrderTakenAsync() => await _hubContext.Clients.All.SendAsync("OrderTaken");
        public async Task SendOrderReleasedAsync() => await _hubContext.Clients.All.SendAsync("OrderReleased");
        public async Task SendOrderResetAsync() => await _hubContext.Clients.All.SendAsync("OrderReset");
        public async Task SendReviewTakenAsync() => await _hubContext.Clients.All.SendAsync("ReviewTaken");
        public async Task SendReviewReleasedAsync() => await _hubContext.Clients.All.SendAsync("ReviewReleased");
        public async Task SendReviewResetAsync() => await _hubContext.Clients.All.SendAsync("ReviewReset");
        public async Task SendEmployeeOnShiftAsync() => await _hubContext.Clients.All.SendAsync("EmployeeOnShift");
        public async Task SendEmployeeOffShiftAsync() => await _hubContext.Clients.All.SendAsync("EmployeeOffShift");
    }
}