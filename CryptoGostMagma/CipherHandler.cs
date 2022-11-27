using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoGostMagma
{
    public class CipherHandler
    {
        /// <summary>
        /// Encrypt string.
        /// </summary>
        /// <param name="plainText">text to encrypt</param>
        /// <param name="Key">encryption key for cipher</param>
        /// <param name="IV">initialization vector</param>
        /// <param name="_cipher">implementation of ciper algorithm</param>
        /// <returns>encrypted bytes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV, ICipherAlgorithm _cipher)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = new CFBTransform(Key, IV, true, _cipher);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        /// <summary>
        /// Decrypt bytes.
        /// </summary>
        /// <param name="cipherText">encrypted bytes</param>
        /// <param name="Key">encryption key for cipher</param>
        /// <param name="IV">initialization vector</param>
        /// <param name="_cipher">implementation of ciper algorithm</param>
        /// <returns>decrypted text</returns>
        public string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV, ICipherAlgorithm _cipher)
        {
            // Check arguments.
            ValidateArguments(cipherText, Key, IV);

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = new CFBTransform(Key, IV, false, _cipher);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }

            return plaintext;
        }

        private void ValidateArguments(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
        }
    }
}
