using System.Security.Cryptography;
using System.Text;

namespace IotFlow.Utilities
{
    public static class StringHasher
    {
        public static string HashStringSHA256(string input)
        {
            using HashAlgorithm algorithm = SHA256.Create();

            byte[] encryptedByteArray = algorithm.ComputeHash(Encoding.ASCII.GetBytes(input));

            return Convert.ToBase64String(encryptedByteArray);
        }
    }
}
