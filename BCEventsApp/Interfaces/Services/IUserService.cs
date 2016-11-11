using System.Collections.Generic;
using System.Threading.Tasks;
using BCEventsApp.Models;

namespace BCEventsApp.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<Calendar>> GetAllCalendars(string userId);

        Task<int> GetPrimaryCalendarId(string userId);

        Task<Dictionary<int, Event>> GetAllSubscribedEvents(string userId);

        Task<int> CreateEvent(string userId, Event e);

        Task ModifyEvent(Event e);

        Task<int> CreateCalendar(string userId, Calendar calendar);

        Task DeleteEvent(int calendarId, int eventId);

        Task SubscribeEvent(int eventId, string userId);

        Task UnsubscribeEvent(int eventId, string userId);

        Task SubscribeCalendar(int calendarId, string userId);

        Task UnsubscribeCalendar(int calendarId, string userId);

        Task PublishEvent(int eventId);

        Task UnpublishEvent(int eventId);

        Task PublishCalendar(int calendarId);

        Task UnpublishCalendar(int calendarId);

        Task DeleteCalendar(int calendarId);
    }
}