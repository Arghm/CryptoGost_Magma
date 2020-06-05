using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoGost
{
    class Program
    {
        static void Main(string[] args)
        {
            string plainText = "велоцираптор";
            byte[] bPlainText = Encoding.UTF8.GetBytes(plainText);
            //byte[] _key = Utils.Unpack("33206D54326C656820657369626E7373796761207474676965686573733D2C20");
            byte[] _ref_plain = Encoding.UTF8.GetBytes("01234567"); //Utils.Unpack("4849505152535455");

            Magma cipher = new Magma();
            //cipher.SetKey(_ref_key);
            //byte[] enc_result = cipher.Encrypt(_ref_plain);
            //byte[] dec_result = cipher.Encrypt(enc_result);

            //byte[] plainText = new byte[SAMPLE_SIZE_KB * 1024];
            byte[] _key = new byte[cipher.KeyLength];
            byte[] _iv = new byte[cipher.BlockSize];

            Random rng = new Random();
            //rng.NextBytes(plainText);
            rng.NextBytes(_key);
            rng.NextBytes(_iv);

            byte[] encrypted = EncryptStringToBytes(plainText, _key, _iv, cipher);

            string roundtrip = DecryptStringFromBytes(encrypted, _key, _iv, cipher);

            Console.WriteLine("plain text: '" + plainText + "'");
            Console.WriteLine("enc   text: " + Utils.PackToHexString(encrypted));
            Console.WriteLine("dec   text: '" + roundtrip + "'");
            Console.WriteLine("IsEqual = " + plainText.Equals(roundtrip).ToString());

            Console.ReadKey();
        }

        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV, ICipherAlgorithm _cipher)
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

        static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV, ICipherAlgorithm _cipher)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

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
    }
}
