using Isopoh.Cryptography.Argon2;
using seed_store_api.Store.Support.PasswordHash.Interfaces;

namespace seed_store_api.Store.Support.PasswordHash.Services
{
    public class PasswordHashService : IPasswordHashService
    {
        public string Hash(string password)
        {
            return Argon2.Hash(password);
        }

        public bool Verify(string password, string hash)
        {
            return Argon2.Verify(hash, password);
        }
    }
}