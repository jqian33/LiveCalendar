using System.Threading.Tasks;
using BCEventsApp.Cache;
using BCEventsApp.Interfaces.Services;
using BCEventsApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.MockData;

namespace UnitTest.Services
{
    [TestClass]
    public class AuthorizationServiceTest
    {
        private IAuthorizationService authorizationService;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockCalendarActions = new MockCalendarActions();
            var mockGlobalActions = new MockGlobalActions();
            var mockEventActions = new MockEventActions();
            MainCache mainCache = new MainCache(mockEventActions, mockGlobalActions, mockCalendarActions);
            Task.Run(() => mainCache.Initialize()).Wait();
            authorizationService = new AuthorizationService(mainCache);
        }

        [TestMethod]
        public void TestCalendarOwner()
        {
            Assert.IsTrue(authorizationService.CalendarOwner("testa", 1));
            Assert.IsFalse(authorizationService.CalendarOwner("testb", 1));
            Assert.IsFalse(authorizationService.CalendarOwner("invalidUser", 1));
            Assert.IsFalse(authorizationService.CalendarOwner("testa", 99999));
        }

        [TestMethod]
        public void TestEventOwner()
        {
            Assert.IsTrue(authorizationService.EventOwner("testa", 1));
            Assert.IsTrue(authorizationService.EventOwner("testa", 2));
            Assert.IsFalse(authorizationService.EventOwner("testb", 1));
            Assert.IsFalse(authorizationService.EventOwner("invalidUser", 1));
            Assert.IsFalse(authorizationService.EventOwner("testa", 99999));
        }
    }
}
