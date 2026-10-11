using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

namespace EventFlowAPI.DB.Extensions
{
    public class SeedPasswordHasher
    {
        private const int PrfHmacSha512 = 2;
        private const int IterationCount = 100000;
        private const int SaltSize = 16;
        private const int SubkeySize = 32;
        private const int HeaderSize = 13;

        public string Hash(string saltSeed, string password)
        {
            var salt = SHA256.HashData(Encoding.UTF8.GetBytes(saltSeed))[..SaltSize];
            var subkey = Rfc2898DeriveBytes.Pbkdf2(password, salt, IterationCount, HashAlgorithmName.SHA512, SubkeySize);

            var hash = new byte[HeaderSize + SaltSize + SubkeySize];
            hash[0] = 0x01;
            BinaryPrimitives.WriteUInt32BigEndian(hash.AsSpan(1), PrfHmacSha512);
            BinaryPrimitives.WriteUInt32BigEndian(hash.AsSpan(5), IterationCount);
            BinaryPrimitives.WriteUInt32BigEndian(hash.AsSpan(9), SaltSize);
            salt.CopyTo(hash, HeaderSize);
            subkey.CopyTo(hash, HeaderSize + SaltSize);

            return Convert.ToBase64String(hash);
        }
    }
}
