using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCEventsApp.Interfaces.Database;
using BCEventsApp.Models;

namespace UnitTest.MockData
{
    class MockGlobalActions: IGlobalActions
    {
        public Task DeleteUser(string username)
        {
            return Task.FromResult(0);
        }

        public Task AddUser(User user, string hash)
        {
            return Task.FromResult(0);
        }

        public async Task<Dictionary<int, Calendar>> GetAllCalendars()
        {
            var calendarList = new Dictionary<int, Calendar>();
            var calendar = new Calendar()
            {
                Id = 1,
                Title = "Test Calendar"
            };
            calendarList.Add(calendar.Id, calendar);
            return await Task.Run(() => calendarList);
        }

        public async Task<Calendar> GetCalendarById(int calendarId)
        {
            var calendar = new Calendar();
            return await Task.Run(() => calendar);
        }

        public async Task<List<int>> GetPublishedCalendarIds()
        {
            var idList = new List<int>();
            return await Task.Run(() => idList);
        }

        public async Task<Dictionary<string, User>> GetAllUsers()
        {
            var userList = new Dictionary<string, User>();
            var testa = new User()
            {
                Id = "testa",
                FirstName = "Alpha",
                LastName = "Test"
            };
            var testb = new User()
            {
                Id = "testb",
                FirstName = "Beta",
                LastName = "Test"
            };
            userList.Add(testa.Id, testa);
            userList.Add(testb.Id, testb);
            return await Task.Run(() => userList);
        }

        public async Task<Dictionary<int, Tag>> GetAllTags()
        {
            var tagList = new Dictionary<int, Tag>();
            return await Task.Run(() => tagList);
        }

        public async Task<Dictionary<int, Event>> GetAllEvents()
        {
            var eventList = new Dictionary<int, Event>();
            var event1 = new Event()
            {
                Id = 1,
                Title = "Test Event 1",
                Attendees = new List<User>(),
                Tags = new List<Tag>(),
                Start = DateTime.Now,
                Duration = 60
            };
            var event2 = new Event()
            {
                Id = 2,
                Title = "Test Event 2",
                Attendees = new List<User>(),
                Tags = new List<Tag>(),
                Start = DateTime.Now.AddHours(2),
                Duration = 30
            };
            eventList.Add(event1.Id, event1);
            eventList.Add(event2.Id, event2);
            return await Task.Run(() => eventList);
        }

        public async Task<List<int>> GetPublishedEventIds()
        {
            var idList = new List<int>();
            idList.Add(2);
            return await Task.Run(() => idList);
        }

        public async Task<bool> VerifyPasswordHash(string userId, string passwordHash)
        {
            bool verified = userId == "testa" && passwordHash == "5be0888bbe2087f962fee5748d9cf52e37e4c6a24af79675ff7e1ca0a1b12739";
            return await Task.Run(() => verified);
        }
    }
}
