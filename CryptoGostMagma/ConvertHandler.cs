using System;
using System.Linq;

namespace CryptoGostMagma
{
    public static class ConvertHandler
    {
        public static string BytesToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}
