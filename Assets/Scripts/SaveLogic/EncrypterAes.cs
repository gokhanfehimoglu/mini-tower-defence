using System.IO;
using System.Security.Cryptography;

namespace MiniTowerDefence.SaveLogic
{
    public static class EncrypterAes
    {
        private static readonly byte[] Key =
        {
            132, 220, 217, 217, 98, 24, 228, 16, 245, 109, 148, 172, 255, 213, 184, 14,
            215, 124, 208, 15, 214, 35, 20, 170, 167, 238, 34, 127, 131, 49, 251, 200
        };

        private static readonly byte[] Iv =
            { 116, 64, 191, 111, 23, 3, 113, 119, 231, 121, 152, 112, 79, 32, 114, 121 };

        public static byte[] EncryptStringToBytes_Aes(string plainText)
        {
            if (plainText == null || plainText.Length <= 0)
            {
                return null;
            }

            using var aesAlg = Aes.Create();
            aesAlg.Key = Key;
            aesAlg.IV = Iv;

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            var encrypted = msEncrypt.ToArray();

            return encrypted;
        }

        public static string DecryptStringFromBytes_Aes(byte[] cipherText)
        {
            if (cipherText == null || cipherText.Length <= 0)
            {
                return null;
            }

            using var aesAlg = Aes.Create();
            aesAlg.Key = Key;
            aesAlg.IV = Iv;

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var msDecrypt = new MemoryStream(cipherText);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            var plaintext = srDecrypt.ReadToEnd();

            return plaintext;
        }
    }
}