using System.Security.Cryptography;

namespace ProjectsRequirements_API.Security
{
    public class PasswordHasher
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32;  // 256 bit
        private const int Iterations = 100_000; // Modern recommendation

        public static byte[] Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var key = pbkdf2.GetBytes(KeySize);

            var hashBytes = new byte[SaltSize + KeySize];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
            Buffer.BlockCopy(key, 0, hashBytes, SaltSize, KeySize);

            return hashBytes;
        }

        public static bool Verify(string password, byte[] storedHash)
        {
            var salt = new byte[SaltSize];
            Buffer.BlockCopy(storedHash, 0, salt, 0, SaltSize);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var key = pbkdf2.GetBytes(KeySize);

            return CryptographicOperations.FixedTimeEquals(
                storedHash.AsSpan(SaltSize, KeySize),
                key
            );
        }
    }
}
