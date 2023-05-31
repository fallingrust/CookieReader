using CookieReader.Entity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace CookieReader
{
    public class CookieReaderUtil
    {
        private static string? _encKey = "";
        private static byte[]? _decKey = Array.Empty<byte>();
        public static bool CheckFileExists()
        {            
            if (!File.Exists(Consts.CookieDbPath))
            {
                throw new FileNotFoundException("CookieDb NOT EXISTS", Consts.CookieDbPath);
            }
            if (!File.Exists(Consts.LocalStatePath))
            {
                throw new FileNotFoundException("Local State NOT EXISTS", Consts.LocalStatePath);
            }
            else
            {
                var localState = File.ReadAllText(Consts.LocalStatePath);
                var root_node = JObject.Parse(localState);
                if (root_node != null)
                {
                    var os_crypt_node = root_node["os_crypt"];
                    if(os_crypt_node != null)
                    {
                        _encKey = os_crypt_node["encrypted_key"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(_encKey))
                        {
                            if (OperatingSystem.IsWindows())
                            {
                                _decKey = ProtectedData.Unprotect(Convert.FromBase64String(_encKey).Skip(5).ToArray(), null, DataProtectionScope.LocalMachine);
                            }
                        }
                    }                   
                }                
            }
            return true;
        }

        public static List<Cookies>? GetCookies()
        {
            using var ctx = new CookieDbCtx();
            var cookies = ctx.Cookies?.AsNoTracking().ToList();
            if (cookies != null)
            {
                foreach (var cookie in cookies)
                {
                    DecryptValue(cookie);
                }
            }
            return cookies;
        }
        public static List<Cookies>? GetCookies(Expression<Func<Cookies, bool>> expression)
        {
            using var ctx = new CookieDbCtx();
            var cookies = ctx.Cookies?.AsNoTracking().Where(expression).ToList();
            if (cookies != null)
            {
                foreach (var cookie in cookies)
                {
                    DecryptValue(cookie);
                }
            }
            return cookies;
        }       

        private static Cookies DecryptValue(Cookies cookies)
        {
            if (_decKey != null)
            {
                var value = DecryptWithKey(cookies.EncryptedValue, _decKey, 3);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    cookies.Value = value;
                }                
            }
            return cookies;
        }

        private static string? DecryptWithKey(byte[] message, byte[] key, int nonSecretPayloadLength)
        {
            const int KEY_BIT_SIZE = 256;
            const int MAC_BIT_SIZE = 128;
            const int NONCE_BIT_SIZE = 96;

            if (key == null || key.Length != KEY_BIT_SIZE / 8)
                throw new ArgumentException(string.Format("Key needs to be {0} bit!", KEY_BIT_SIZE), "key");
            if (message == null || message.Length == 0)
                throw new ArgumentException("Message required!", "message");

            using var cipherStream = new MemoryStream(message);
            using var cipherReader = new BinaryReader(cipherStream);
            var nonSecretPayload = cipherReader.ReadBytes(nonSecretPayloadLength);
            var nonce = cipherReader.ReadBytes(NONCE_BIT_SIZE / 8);
            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key), MAC_BIT_SIZE, nonce);
            cipher.Init(false, parameters);
            var cipherText = cipherReader.ReadBytes(message.Length);
            var plainText = new byte[cipher.GetOutputSize(cipherText.Length)];
            try
            {
                var len = cipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0);
                cipher.DoFinal(plainText, len);
            }
            catch (InvalidCipherTextException)
            {
                return null;
            }
            return Encoding.Default.GetString(plainText);
        }
    }
}
