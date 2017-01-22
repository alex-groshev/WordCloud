using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace WordCloud.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _salt;
        private readonly CspParameters _csp;

        public EncryptionService()
        {
            _salt = Salt(ConfigurationManager.AppSettings["user.guid"]);
            _csp = new CspParameters
            {
                KeyContainerName = Encoding.UTF8.GetString(Salt(ConfigurationManager.AppSettings["container"])),
                Flags = CspProviderFlags.UseMachineKeyStore
            };
        }

        public Tuple<byte[], byte[]> Encrypt(string word)
        {
            return new Tuple<byte[], byte[]>(Key(_salt, word), EncryptToByteArray(word));
        }

        public string Decrypt(byte[] data)
        {
            return DecryptToString(data);
        }

        private byte[] EncryptToByteArray(string s)
        {
            using (var rsa = new RSACryptoServiceProvider(_csp))
            {
                return rsa.Encrypt(new UnicodeEncoding().GetBytes(s), false);
            }
        }

        private string DecryptToString(byte[] data)
        {
            using (var rsa = new RSACryptoServiceProvider(_csp))
            {
                return new UnicodeEncoding().GetString(rsa.Decrypt(data, false));
            }
        }

        private byte[] Salt(string guid)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(Encoding.ASCII.GetBytes(guid.Replace("-", string.Empty)));
            }
        }

        private byte[] Key(byte[] salt, string word)
        {
            using (var db = new Rfc2898DeriveBytes(word, salt))
            {
                return db.GetBytes(16);
            }
        }
    }
}