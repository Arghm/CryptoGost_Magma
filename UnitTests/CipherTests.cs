using CryptoGostMagma;
using System;
using Xunit;

namespace UnitTests
{
    public class CipherTests
    {
        [Fact]
        public void MagmaEncryptDecrypt_Success()
        {
            // Arrange
            const string plainText = "dis is a workin cipher algorithm";

            Magma cipher = new Magma();

            byte[] _key = new byte[cipher.KeyLength];
            byte[] _iv = new byte[cipher.BlockSize];
            var rnd = new Random();
            rnd.NextBytes(_key);
            rnd.NextBytes(_iv);

            var sut = new CipherHandler();

            // Act

            byte[] encrypted = sut.EncryptStringToBytes(plainText, _key, _iv, cipher);

            string encryptedText = sut.DecryptStringFromBytes(encrypted, _key, _iv, cipher);

            // Assert
            Assert.Equal(plainText, encryptedText);
        }
    }
}
