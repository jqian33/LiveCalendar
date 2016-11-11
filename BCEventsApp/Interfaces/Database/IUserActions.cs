using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BCEventsApp.Interfaces.Database
{
    public interface IUserActions
    {
        Task<List<int>> GetAllCalendars(string userId);

        Task<int> GetPrimaryCalendarId(string userId);

        Task<List<int>> GetAllSubscribedEvents(string userId);

        Task SubscribeCalendar(string userId, int calendarId);

        Task UnsubscribeCalendar(string userId, int calendarId);

        Task SubscribeEvent(string userId, int eventId);

        Task AttendEvent(string userId, int eventId);

        Task WithdrawEvent(string userId, int eventId);

        Task UnsubscribeEvent(string userId, int eventId);

        Task<int> CreateCalendar(string userId, string title, string description);

        Task<int> CreateEvent(string userId, string title, string description, string location, DateTime startTime, int duration,
            DateTime endTime, string rRule, int calendarId);

        Task ModifyEventTitle(int eventId, string title);

        Task ModifyEventDescription(int eventId, string description);

        Task ModifyEventLocation(int eventId, string location);

        Task ModifyEventStartTime(int eventId, DateTime startTime);

        Task ModifyEventDuration(int eventId, int duration);

        Task ModifyEventEndTime(int eventId, DateTime endTime);

        Task ModifyEventRRule(int eventId, string rRule);

        Task PublishEvent(int eventId);

        Task UnpublishEvent(int eventId);

        Task DeleteEvent(int eventId);

        Task DeleteCalendar(int calendarId);

        Task UnpublishCalendar(int calendarId);

        Task PublishCalendar(int calendarId);

    }
}
