namespace SeedStore.Support.Store.Notifications.Interfaces
{
    public interface INotificationService
    {
        Task SendNewOrderAsync();
        Task SendNewReviewAsync();
        Task SendOrderTakenAsync();
        Task SendOrderReleasedAsync();
        Task SendOrderResetAsync();
        Task SendReviewTakenAsync();
        Task SendReviewReleasedAsync();
        Task SendReviewResetAsync();
        Task SendEmployeeOnShiftAsync();
        Task SendEmployeeOffShiftAsync();
    }
}
