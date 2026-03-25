using SeedStore.Support.Store.Payment.Models;

namespace SeedStore.Support.Store.Payment.Interfaces
{
    public interface IPaymentService
    {
        (string data, string signature) GetPaymentData(string orderNumber, decimal amount, string description);
        bool ValidateCallback(string data, string signature);
        LiqPayCallbackDataModel DecodeCallbackData(string data);
    }
}