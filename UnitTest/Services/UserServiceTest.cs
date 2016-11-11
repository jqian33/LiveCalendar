using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCEventsApp.Cache;
using BCEventsApp.Interfaces.Services;
using BCEventsApp.Models;
using BCEventsApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.MockData;

namespace UnitTest.Services
{
    [TestClass]
    public class UserServiceTest
    {
        private IUserService userService;
        private MainCache mainCache;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockCalendarActions = new MockCalendarActions();
            var mockGlobalActions = new MockGlobalActions();
            var mockEventActions = new MockEventActions();
            var mockUserActions = new MockUserActions();
            mainCache = new MainCache(mockEventActions, mockGlobalActions, mockCalendarActions);
            Task.Run(() => mainCache.Initialize()).Wait();
            userService = new UserService(mainCache, mockUserActions);
        }

        [TestMethod]
        public async Task TestPrimaryCalendarId()
        {
            var id = await userService.GetPrimaryCalendarId("testa");
            Assert.AreEqual(id, 1);
        }

        [TestMethod]
        public async Task TestGetAllCalendars()
        {
            var idList = await userService.GetAllCalendars("testa");
            Assert.AreEqual(idList.Count, 1);
        }

        [TestMethod]
        public async Task TestGetAllSubscribedEvents()
        {
            var idList = await userService.GetAllSubscribedEvents("testa");
            Assert.AreEqual(idList.Count, 0);
        }

        [TestMethod]
        public async Task TestCreateEvent()
        {
            var testEvent = new Event()
            {
                Title = "Test Event Creation",
                Description = "For unit test event creations",
                Location = "Anywhere",
                Start = DateTime.Now,
                Duration = 30,
                CalendarId = 1,
                Attendees = new List<User>(),
                Tags = new List<Tag>(),
            };
            await userService.CreateEvent("testa", testEvent);
            var events = mainCache.GetEvents();
            Assert.IsTrue(events.ContainsKey(999));
        }

        [TestMethod]
        public async Task TestModifyEvent()
        {
            var modifiedEvent = new Event()
            {
                Id = 1,
                Title = "Test Event 1 Edited",
                Attendees = new List<User>(),
                Tags = new List<Tag>(),
                CalendarId = 1
            };
            await userService.ModifyEvent(modifiedEvent);
            var events = mainCache.GetEvents();
            Assert.AreEqual(events[1].Title, "Test Event 1 Edited");
        }

        [TestMethod]
        public async Task TestPublishEvent()
        {
            await userService.PublishEvent(1);
            var publicEvents = mainCache.GetPublicEvents();
            Assert.IsTrue(publicEvents.ContainsKey(1));
        }

        [TestMethod]
        public async Task TestUnpublishEvent()
        {
            await userService.UnpublishEvent(2);
            var publicEvents = mainCache.GetPublicEvents();
            Assert.IsFalse(publicEvents.ContainsKey(2));
        }
    }
}
