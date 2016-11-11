using System.Collections.Generic;
using System.Threading.Tasks;

namespace BCEventsApp.Interfaces.Database
{
    public interface ICalendarActions
    {
        Task<List<int>> GetTagIds(int calendarId);

        Task<List<int>> GetEventIds(int calendarId);

        Task<string> GetOwnerId(int calendarId);

        Task InsertTag(int calendarId, string tagName);

        Task RemoveTag(int calendarId, int tagId);
    }
}