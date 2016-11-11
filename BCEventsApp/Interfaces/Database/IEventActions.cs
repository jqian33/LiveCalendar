using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BCEventsApp.Interfaces.Database
{
    public interface IEventActions
    {
        Task<List<int>> GetTagIds(int eventId);

        Task<string> GetOwnerId(int eventId);

        Task<List<string>> GetAttendeeIds(int eventId);

        Task InsertTag(int eventId, string tagName);

        Task RemoveTag(int eventId, int tagId);
    }
}