using SeedStore.Support.Store.Payment.LiqPay.Models;

namespace SeedStore.Support.Store.Payment.LiqPay.Interfaces
{
    public interface ILiqPayService
    {
        (string data, string signature) GetPaymentData(string orderNumber, decimal amount, string description);
        bool ValidateCallback(string data, string signature);
        LiqPayCallbackDataModel DecodeCallbackData(string data);
    }
}