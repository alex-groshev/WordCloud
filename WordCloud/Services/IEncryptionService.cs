using System;

namespace WordCloud.Services
{
    public interface IEncryptionService
    {
        /// <summary>
        /// Encrypt <paramref name="word"/>
        /// </summary>
        /// <param name="word">Word to encrypt</param>
        /// <returns>Tuple of hashed salt and encrypted word</returns>
        Tuple<byte[], byte[]> Encrypt(string word);

        string Decrypt(byte[] data);
    }
}
