using System.Collections.Generic;
using System.Threading.Tasks;
using BCEventsApp.Interfaces.Database;

namespace UnitTest.MockData
{
    class MockCalendarActions: ICalendarActions
    {
        public async Task<List<int>> GetTagIds(int calendarId)
        {
            var idList = new List<int>();
            return await Task.Run(() => idList);
        }

        public async Task<List<int>> GetEventIds(int calendarId)
        {
            var eventIdList = new List<int>();
            if (calendarId == 1)
            {
                eventIdList.Add(1);
                eventIdList.Add(2);
            }
            return await Task.Run(() => eventIdList);
        }

        public async Task<string> GetOwnerId(int calendarId)
        {
            string id = null;

            if (calendarId == 1)
            {
                id = "testa";
            }
            return await Task.Run(() => id);
        }

        public Task InsertTag(int calendarId, string tagName)
        {
            return Task.FromResult(0);
        }

        public Task RemoveTag(int calendarId, int tagId)
        {
            return Task.FromResult(0);
        }
    }
}
