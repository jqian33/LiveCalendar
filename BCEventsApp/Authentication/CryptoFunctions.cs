using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BCEventsApp.Models;
using Newtonsoft.Json;

namespace BCEventsApp.Authentication
{
    public class CryptoFunctions
    {
        public static string GetPasswordHash(string value)
        {
            var sb = new StringBuilder();

            var hash = SHA256.Create();
            var enc = Encoding.UTF8;
            var result = hash.ComputeHash(enc.GetBytes(value));

            foreach (var b in result)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public static string GenerateToken(string username, string key)
        {
            Token token = new Token(username);
            token.CreateSignature(key);
            return token.GetSignedToken();
        }

        // Return TokenPayload object if token is verified, else return null
        public static TokenPayload VerifyToken(string tokenString, string key)
        {
            var rsa = new RSACryptoServiceProvider();
            var sha1 = new SHA1CryptoServiceProvider();
            rsa.FromXmlString(key);
            try
            {
                var buffer = Convert.FromBase64String(tokenString);
                var payload = buffer.Take(buffer.Length - 128).ToArray();
                var jsonStr = Encoding.UTF8.GetString(payload, 0, payload.Length);
                var tokenPayload = JsonConvert.DeserializeObject<TokenPayload>(jsonStr);
                var sig = buffer.Skip(payload.Length).Take(128).ToArray();
                if (rsa.VerifyData(payload, sha1, sig))
                {
                    return tokenPayload.ExpireTime > DateTime.Now ? tokenPayload : null;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        // Set key for token generation
        public static string GenerateKey()
        {
            var rsa = new RSACryptoServiceProvider();
            return rsa.ToXmlString(true);
        }
    }
}