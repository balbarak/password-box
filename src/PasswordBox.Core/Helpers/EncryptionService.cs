using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PasswordBox.Core.Helpers
{
    public class EncryptionService
    {
        public string Encrypt(string data, string password)
        {
            string result = null;

            byte[] dataToBeEncrypted = Encoding.UTF8.GetBytes(data);

            using (AesManaged aesAlg = new AesManaged())
            {
                var key = GenerateKey(password);

                aesAlg.GenerateIV();
                aesAlg.Key = key;
                var iv = aesAlg.IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                var encryptedBytes = encryptor.TransformFinalBlock(dataToBeEncrypted, 0, dataToBeEncrypted.Length);

                byte[] bodyBytes = new byte[encryptedBytes.Length + iv.Length];

                Buffer.BlockCopy(iv, 0, bodyBytes, 0, iv.Length);
                Buffer.BlockCopy(encryptedBytes, 0, bodyBytes, iv.Length, encryptedBytes.Length);

                result = Convert.ToBase64String(bodyBytes);
            }


            return result;
        }

        public string Decrypt(string data, string password)
        {
            string result = null;
            byte[] dataToBeDecrypted = Convert.FromBase64String(data);

            using (AesManaged aesAlg = new AesManaged())
            {
                var key = GenerateKey(password);

                aesAlg.Key = key;
                aesAlg.IV = dataToBeDecrypted.Skip(0).Take(16).ToArray();

                var dataToDecrypt = dataToBeDecrypted.Skip(16).ToArray();

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                var decryptedBytes = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);

                result = Encoding.UTF8.GetString(decryptedBytes);
            }

            return result;
        }

        private byte[] GenerateKey(string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);

            SHA256 sha512 = SHA256.Create();

            return sha512.ComputeHash(keyBytes);

        }
    }
}
