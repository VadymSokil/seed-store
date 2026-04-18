using SeedStore.Store.Account.Models;

namespace SeedStore.Store.Account.Interfaces
{
    public interface IAccountService
    {
        Task<AccountInfoResponseModel?> GetAccountInfoAsync(int accountId);
        Task<string> ChangeNameAsync(int accountId, ChangeNameModel model);
        Task<string> ChangeEmailAsync(int accountId, string newEmail);
        Task<string> ConfirmEmailChangeAsync(int accountId, ConfirmEmailChangeModel model);
        Task<string> ResendEmailChangeCodeAsync(int accountId);
        Task<string> ChangePhoneAsync(int accountId, string? phoneNumber);
        Task<string> ChangePasswordAsync(int accountId, ChangePasswordModel model);
        Task<string> DeleteAccountAsync(int accountId);
    }
}
