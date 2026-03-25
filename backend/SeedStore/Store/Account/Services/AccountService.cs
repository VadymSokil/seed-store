using SeedStore.Database.Entities.Store.Account;
using SeedStore.Store.Account.Interfaces;
using SeedStore.Store.Account.Models;
using SeedStore.Support.General.PasswordHash.Interfaces;
using SeedStore.Support.General.TokenGeneration.Interfaces;
using SeedStore.Support.Store.Email.Interfaces;

namespace SeedStore.Store.Account.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailService _emailService;
        private readonly ITokenGenerationService _tokenGenerationService;
        private readonly IPasswordHashService _passwordHashService;

        public AccountService(IAccountRepository accountRepository, IEmailService emailService, ITokenGenerationService tokenGenerationService, IPasswordHashService passwordHashService)
        {
            _accountRepository = accountRepository;
            _emailService = emailService;
            _tokenGenerationService = tokenGenerationService;
            _passwordHashService = passwordHashService;
        }

        public async Task<AccountInfoResponseModel?> GetAccountInfoAsync(int accountId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account == null)
                return null;

            return new AccountInfoResponseModel
            {
                FirstName = account.FirstName,
                LastName = account.LastName,
                MiddleName = account.MiddleName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber
            };
        }

        public async Task<string> ChangeNameAsync(int accountId, ChangeNameModel model)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account == null)
                return "not_found";

            await _accountRepository.UpdateAccountNameAsync(accountId, model.FirstName, model.LastName, model.MiddleName);

            return "ok";
        }

        public async Task<string> ChangeEmailAsync(int accountId, string newEmail)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account == null)
                return "not_found";

            var emailExists = await _accountRepository.EmailExistsAsync(newEmail);

            if (emailExists)
                return "email_taken";

            var code = _tokenGenerationService.GenerateVerificationCode();

            var entity = new EmailChangeRequestEntity
            {
                AccountId = accountId,
                NewEmail = newEmail,
                Code = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            await _accountRepository.UpsertEmailChangeRequestAsync(entity);

            await _emailService.SendEmailAsync(
                newEmail,
                "Зміна електронної пошти",
                $"<p>Ваш код підтвердження зміни пошти: <b>{code}</b></p><p>Код дійсний 15 хвилин.</p>"
            );

            return "ok";
        }

        public async Task<string> ConfirmEmailChangeAsync(int accountId, ConfirmEmailChangeModel model)
        {
            var request = await _accountRepository.GetEmailChangeRequestAsync(accountId, model.NewEmail, model.Code);

            if (request == null)
                return "invalid_code";

            if (request.ExpiresAt < DateTime.UtcNow)
                return "code_expired";

            await _accountRepository.UpdateAccountEmailAsync(accountId, model.NewEmail);
            await _accountRepository.DeleteEmailChangeRequestAsync(request);

            return "ok";
        }

        public async Task<string> ChangePhoneAsync(int accountId, string? phoneNumber)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account == null)
                return "not_found";

            await _accountRepository.UpdateAccountPhoneAsync(accountId, phoneNumber);

            return "ok";
        }

        public async Task<string> ChangePasswordAsync(int accountId, ChangePasswordModel model)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account == null)
                return "not_found";

            if (!_passwordHashService.Verify(model.OldPassword, account.PasswordHash))
                return "invalid_password";

            var passwordHash = _passwordHashService.Hash(model.NewPassword);
            await _accountRepository.UpdateAccountPasswordAsync(accountId, passwordHash);

            return "ok";
        }

        public async Task<string> DeleteAccountAsync(int accountId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account == null)
                return "not_found";

            await _accountRepository.DeleteAccountAsync(accountId);

            return "ok";
        }
    }
}