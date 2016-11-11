using BCEventsApp.Cache;
using BCEventsApp.Interfaces.Services;

namespace BCEventsApp.Services
{
    public class AuthorizationService: IAuthorizationService
    {
        private readonly MainCache mainCache;

        public AuthorizationService(MainCache mainCache)
        {
            this.mainCache = mainCache;
        }
        public bool CalendarOwner(string userId, int calendarId)
        {
            var userList = mainCache.GetUsers();
            var calendarList = mainCache.GetCalendars();
            if (userList.ContainsKey(userId) && calendarList.ContainsKey(calendarId))
            {
                var user = userList[userId];
                var calendar = calendarList[calendarId];
                return calendar.Owner.Id == user.Id;
            }
            return false;
        }

        public bool EventOwner(string userId, int eventId)
        {
            var userList = mainCache.GetUsers();
            var eventList = mainCache.GetEvents();
            if (userList.ContainsKey(userId) && eventList.ContainsKey(eventId))
            {
                var user = userList[userId];
                var e = eventList[eventId];
                return e.Owner.Id == user.Id;
            }
            return false;
        }
    }
}