using Isopoh.Cryptography.Argon2;
using SeedStore.Support.General.PasswordHash.Interfaces;

namespace SeedStore.Support.General.PasswordHash.Services
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