using CFAIProcessor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Services
{
    public class PBKDF2PasswordService : IPasswordService
    {
        private const int _keySize = 64;
        private const int _iterations = 350000;
        private HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

        public bool IsAllowed(string password)
                => !String.IsNullOrEmpty(password) && password.Length > 6 && password.Length < 30;

        public string[] Encrypt(string password)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(_keySize);

            return new string[] { GetEncrypted(password, saltBytes), Convert.ToHexString(saltBytes) };
        }

        private string GetEncrypted(string password, byte[] saltBytes)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), saltBytes,
                 _iterations, _hashAlgorithm, _keySize);

            return Convert.ToHexString(hash);
        }

        public bool IsValid(string encryptedPassword, string inputPassword, string salt)
        {
            var newEncryptedPassword = GetEncrypted(inputPassword, Convert.FromHexString(salt));

            return encryptedPassword.Equals(newEncryptedPassword);
        }
    }
}
