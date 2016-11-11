using System;
using System.Linq;
using System.Text;
using BCEventsApp.Authentication;
using BCEventsApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTest.Authentication
{
    [TestClass]
    public class CryptoFunctionsTest
    {
        private string key;
        private string token;

        [TestInitialize]
        public void TestInitialize()
        {
            key = CryptoFunctions.GenerateKey();
            token = CryptoFunctions.GenerateToken("testa", key);
        }

        [TestMethod]
        public void TestVerifyToken()
        {
            var tokenPayload = CryptoFunctions.VerifyToken(token, key);
            Assert.AreEqual(tokenPayload.UserId, "testa");
        }

        [TestMethod]
        public void TestVerifyInvalidToken()
        {
            var token2 = CryptoFunctions.GenerateToken("testc", key);
            var buffer = Convert.FromBase64String(token2);
            var payload = buffer.Take(buffer.Length - 128).ToArray();
            var sig = buffer.Skip(payload.Length).Take(128).ToArray();
            var tokenPayloadImposter = new TokenPayload()
            {
                UserId = "testa",
                ExpireTime = DateTime.Now.AddMinutes(120)
            };
            var serializedTokenPayload = JsonConvert.SerializeObject(tokenPayloadImposter);
            payload = Encoding.UTF8.GetBytes(serializedTokenPayload);
            var newByteArray = new byte[payload.Length + sig.Length];
            payload.CopyTo(newByteArray, 0);
            sig.CopyTo(newByteArray, payload.Length);
            var fakeToken = Convert.ToBase64String(newByteArray);
            var result = CryptoFunctions.VerifyToken(fakeToken, key);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestPasswordHash()
        {
            var hash = CryptoFunctions.GetPasswordHash("asdfghjk");
            Assert.AreEqual(hash, "5be0888bbe2087f962fee5748d9cf52e37e4c6a24af79675ff7e1ca0a1b12739");
        }
    }
}
