using System.Collections.Generic;
using System.Threading.Tasks;
using BCEventsApp.Interfaces.Database;

namespace UnitTest.MockData
{
    class MockEventActions: IEventActions
    {
        public async Task<List<int>> GetTagIds(int eventId)
        {
            var idList = new List<int>();
            return await Task.Run(() => idList);
        }

        public async Task<string> GetOwnerId(int eventId)
        {
            string id = null;
            if (eventId == 1 || eventId == 2)
            {
                id = "testa";
            }
            return await Task.Run(() => id);
        }

        public async Task<List<string>> GetAttendeeIds(int eventId)
        {
            var idList = new List<string>();
            return await Task.Run(() => idList);
        }

        public Task InsertTag(int eventId, string tagName)
        {
            return Task.FromResult(0);
        }

        public Task RemoveTag(int eventId, int tagId)
        {
            return Task.FromResult(0);
        }
    }
}
