namespace CryptoGostMagma
{
    /// <summary>
    /// Common interface for all cipher algorithms (https://en.wikipedia.org/wiki/GOST_(block_cipher)).
    /// </summary>
    public interface ICipherAlgorithm
    {
        /// <summary>
        /// Printable cipher name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Key length in bytes
        /// </summary>
        int KeyLength { get; }

        /// <summary>
        /// Block size in bytes
        /// </summary>
        int BlockSize { get; }

        /// <summary>
        /// Set encryption key for cipher instance
        /// </summary>
        /// <param name="key">Key as byte array</param>
        void SetKey (byte[] key);

        /// <summary>
        /// Encrypts block of data using key set before
        /// </summary>
        /// <param name="data">Plain text block</param>
        /// <returns>Cipher text block</returns>
        byte[] Encrypt (byte[] data);
    }
}