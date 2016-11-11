using System.Collections.Generic;
using System.Threading.Tasks;

using BCEventsApp.Models;

namespace BCEventsApp.Interfaces.Database
{
    public interface IGlobalActions
    {
        Task DeleteUser(string username);

        Task AddUser(User user, string hash);

        Task<Dictionary<int, Calendar>> GetAllCalendars();

        Task<Calendar> GetCalendarById(int calendarId);

        Task<List<int>> GetPublishedCalendarIds();

        Task<Dictionary<string, User>> GetAllUsers();

        Task<Dictionary<int, Tag>> GetAllTags();

        Task<Dictionary<int, Event>> GetAllEvents();

        Task<List<int>> GetPublishedEventIds();

        Task<bool> VerifyPasswordHash(string userId, string passwordHash);

    }
}
