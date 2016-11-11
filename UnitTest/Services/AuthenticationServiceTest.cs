using System;
using System.Threading.Tasks;
using BCEventsApp.Cache;
using BCEventsApp.Interfaces.Services;
using BCEventsApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.MockData;

namespace UnitTest.Services
{
    [TestClass]
    public class AuthenticationServiceTest
    {
        private IAuthenticationService authenticationService;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockCalendarActions = new MockCalendarActions();
            var mockGlobalActions = new MockGlobalActions();
            var mockEventActions = new MockEventActions();
            MainCache mainCache = new MainCache(mockEventActions, mockGlobalActions, mockCalendarActions);
            Task.Run(() => mainCache.Initialize()).Wait();
            authenticationService = new AuthenticationService(mainCache, mockGlobalActions);
        }

        [TestMethod]
        public async Task TestAuthenticate()
        {
            Assert.IsTrue(await authenticationService.Authenticate("testa", "asdfghjk"));
            Assert.IsTrue(await authenticationService.Authenticate("tESta", "asdfghjk"));
            Assert.IsFalse(await authenticationService.Authenticate("testa", "qwertyui"));
        }

        [TestMethod]
        public void TestToken()
        {
            var token = authenticationService.IssueToken("testa");
            var expiredToken =
                "eyJVc2VySWQiOiJzdGFya3QiLCJFeHBpcmVUaW1lIjoiMjAxNi0wNC0yN1QxMDoxNTo0Mi42Njg5ODQyLTA1OjAwIn2H+fqMYxQg2Xn1W3nFIvPDWz3TivyhanstHckCo0yDFFcJqfRqXnfZbvUetvakigfz+h02TRt9jbxQmNug9RZggypyIfvQQe0apP7I9VdiSaWiRbZIp5MNMo0GJeIWaR7WcsqH8d/XnKvdVvgKPn+Lxooa4I8sLY7StdRVMK5LfA==";
            Assert.IsNull(authenticationService.VerifyTokenOnInitialize(expiredToken));
            Assert.AreEqual(authenticationService.VerifyTokenOnInitialize(token).UserId, "testa");
            Assert.AreEqual(authenticationService.VerifyToken(token).UserId, "testa");
            Assert.IsTrue(authenticationService.VerifyToken(token).ExpireTime > DateTime.Now);
        }


    }
}
