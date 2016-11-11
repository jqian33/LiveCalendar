using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCEventsApp.Interfaces.Database;

namespace UnitTest.MockData
{
    class MockUserActions: IUserActions
    {
        public async Task<List<int>> GetAllCalendars(string userId)
        {
            var idList = new List<int>();
            if (userId == "testa")
            {
                idList.Add(1);
            }
            return await Task.Run(() => idList);
        }

        public async Task<int> GetPrimaryCalendarId(string userId)
        {
            var id = -1;
            if (userId == "testa")
            {
                id = 1;
            }
            return await Task.Run(() => id);
        }

        public async Task<List<int>> GetAllSubscribedEvents(string userId)
        {
            var idList = new List<int>();
            return await Task.Run(() => idList);
        }

        public Task SubscribeCalendar(string userId, int calendarId)
        {
            return Task.FromResult(0);
        }

        public Task UnsubscribeCalendar(string userId, int calendarId)
        {
            return Task.FromResult(0);
        }

        public Task SubscribeEvent(string userId, int eventId)
        {
            return Task.FromResult(0);
        }

        public Task AttendEvent(string userId, int eventId)
        {
            return Task.FromResult(0);
        }

        public Task WithdrawEvent(string userId, int eventId)
        {
            return Task.FromResult(0);
        }

        public Task UnsubscribeEvent(string userId, int eventId)
        {
            return Task.FromResult(0);
        }

        public async Task<int> CreateCalendar(string userId, string title, string description)
        {
            return await Task.Run(() => 999);
        }

        public async Task<int> CreateEvent(string userId, string title, string description, string location, DateTime startTime, int duration, DateTime endTime,
            string rRule, int calendarId)
        {
            return await Task.Run(() => 999);
        }

        public Task ModifyEventTitle(int eventId, string title)
        {
            return Task.FromResult(0);
        }

        public Task ModifyEventDescription(int eventId, string description)
        {
            return Task.FromResult(0);
        }

        public Task ModifyEventLocation(int eventId, string location)
        {
            return Task.FromResult(0);
        }

        public Task ModifyEventStartTime(int eventId, DateTime startTime)
        {
            return Task.FromResult(0);
        }

        public Task ModifyEventDuration(int eventId, int duration)
        {
            return Task.FromResult(0);
        }

        public Task ModifyEventEndTime(int eventId, DateTime endTime)
        {
            return Task.FromResult(0);
        }

        public Task ModifyEventRRule(int eventId, string rRule)
        {
            return Task.FromResult(0);
        }

        public Task PublishEvent(int eventId)
        {
            return Task.FromResult(0);
        }

        public Task UnpublishEvent(int eventId)
        {
            return Task.FromResult(0);
        }

        public Task DeleteEvent(int eventId)
        {
            return Task.FromResult(0);
        }

        public Task DeleteCalendar(int calendarId)
        {
            return Task.FromResult(0);
        }

        public Task UnpublishCalendar(int calendarId)
        {
            return Task.FromResult(0);
        }

        public Task PublishCalendar(int calendarId)
        {
            return Task.FromResult(0);
        }
    }
}
