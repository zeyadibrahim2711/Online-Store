
using System;
using System.Security.Cryptography;

namespace OnlineStore.Helpers
{
    public static class PasswordHasher
    {
        private const int Iterations = 600000;

        public static string HashPassword(string password)
        {
            byte[] salt = new byte[16];

            using (RandomNumberGenerator rng =
                   RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash;

            using (Rfc2898DeriveBytes pbkdf2 =
                   new Rfc2898DeriveBytes(
                       password,
                       salt,
                       Iterations,
                       HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(32);
            }

            return Iterations + "." +
                   Convert.ToBase64String(salt) + "." +
                   Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(
            string password,
            string storedHash)
        {
            if (string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            string[] parts = storedHash.Split('.');

            if (parts.Length != 3)
                return false;

            int iterations;
            byte[] salt;
            byte[] expectedHash;

            try
            {
                if (!int.TryParse(parts[0], out iterations) ||
                    iterations < 1 ||
                    iterations > 1000000)
                {
                    return false;
                }

                salt = Convert.FromBase64String(parts[1]);
                expectedHash = Convert.FromBase64String(parts[2]);
            }
            catch (FormatException)
            {
                return false;
            }

            if (salt.Length != 16 || expectedHash.Length != 32)
                return false;

            byte[] actualHash;

            using (Rfc2898DeriveBytes pbkdf2 =
                   new Rfc2898DeriveBytes(
                       password,
                       salt,
                       iterations,
                       HashAlgorithmName.SHA256))
            {
                actualHash = pbkdf2.GetBytes(32);
            }

            int difference = 0;

            for (int i = 0; i < expectedHash.Length; i++)
            {
                difference |= expectedHash[i] ^ actualHash[i];
            }

            return difference == 0;
        }
    }
}
