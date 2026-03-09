using seed_store_api.Store.Support.Payment.Models;

namespace seed_store_api.Store.Support.Payment.Interfaces
{
    public interface IPaymentService
    {
        (string data, string signature) GetPaymentData(string orderNumber, decimal amount, string description);
        bool ValidateCallback(string data, string signature);
        LiqPayCallbackDataModel DecodeCallbackData(string data);
    }
}