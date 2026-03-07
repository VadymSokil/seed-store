using seed_store_api.Store.Modules.Account.Models;

namespace seed_store_api.Store.Modules.Account.Interfaces
{
    public interface IAccountService
    {
        Task<AccountInfoResponseModel?> GetAccountInfoAsync(int accountId);
        Task<string> ChangeNameAsync(int accountId, ChangeNameModel model);
        Task<string> ChangeEmailAsync(int accountId, string newEmail);
        Task<string> ConfirmEmailChangeAsync(int accountId, ConfirmEmailChangeModel model);
        Task<string> ChangePhoneAsync(int accountId, string? phoneNumber);
        Task<string> ChangePasswordAsync(int accountId, ChangePasswordModel model);
        Task<string> DeleteAccountAsync(int accountId);
    }
}
