using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BCEventsApp.Models;
using Newtonsoft.Json;

namespace BCEventsApp.Authentication
{
    public class Token
    {
        private byte [] payload;

        // Digital signature
        private byte[] signature;

        // Create token with expiry time set for 3 hours later
        public Token(string data)
        {
            var tokenPayload = new TokenPayload()
            {
                UserId = data,
                ExpireTime = DateTime.Now.AddMinutes(60)
            };
            var serializedTokenPayload = JsonConvert.SerializeObject(tokenPayload);
            payload = Encoding.UTF8.GetBytes(serializedTokenPayload);
        }

        // Create a digital signature
        public void CreateSignature(string key)
        {
            var rsa = new RSACryptoServiceProvider();
            var sha1 = new SHA1CryptoServiceProvider();
            rsa.FromXmlString(key);
            signature = rsa.SignData(payload, sha1);
        }

        // Return signed token as string
        public string GetSignedToken()
        {
            return Convert.ToBase64String(payload.Concat(signature).ToArray());
        }

    }
}