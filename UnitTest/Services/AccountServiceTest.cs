using System.Threading.Tasks;
using BCEventsApp.Cache;
using BCEventsApp.Interfaces.Services;
using BCEventsApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.MockData;

namespace UnitTest.Services
{
    [TestClass]
    public class AccountServiceTest
    {
        private IAccountService accountService;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockCalendarActions = new MockCalendarActions();
            var mockGlobalActions = new MockGlobalActions();
            var mockEventActions = new MockEventActions();
            var mockUserActions = new MockUserActions();
            MainCache mainCache = new MainCache(mockEventActions, mockGlobalActions, mockCalendarActions);
            Task.Run(() => mainCache.Initialize()).Wait();
            accountService = new AccountService(mainCache, mockGlobalActions, mockUserActions);
        }

        [TestMethod]
        public void TestUserIdExists()
        {
            Assert.IsTrue(accountService.UserIdExists("testa"));
            Assert.IsTrue(accountService.UserIdExists("tESta"));
            Assert.IsFalse(accountService.UserIdExists("newUserId"));
        }
    }
}
