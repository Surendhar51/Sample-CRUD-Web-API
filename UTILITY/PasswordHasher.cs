using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UTILITY
{
    public class PasswordHasher
    {
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 20;
        private static readonly int Iterations = 1000;

        public static string HashPassword(string Password)
        {
            byte[] salt;
            rng.GetBytes(salt = new byte[SaltSize]);
            var Key = new Rfc2898DeriveBytes(Password, salt, Iterations);
            var hash = Key.GetBytes(HashSize);

            var HashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt,0, HashBytes ,0, SaltSize);
            Array.Copy(hash,0, HashBytes,SaltSize,HashSize);

            var base64Hash = Convert.ToBase64String(HashBytes);

            return base64Hash;
        }

        public static bool VerifyPassword(string Password, string base64Hash)
        {
            var HashBytes = Convert.FromBase64String(base64Hash);

            var salt = new byte[SaltSize];
            Array.Copy(HashBytes, 0,salt , 0, SaltSize);

            var Key = new Rfc2898DeriveBytes(Password, salt, Iterations);
            byte[] hash = Key.GetBytes(HashSize);
            for (int i = 0; i < HashSize; i++)
            {
                if (HashBytes[i+SaltSize] != hash[i])
                    return false;
            }
            return true;
        }
    }
}
