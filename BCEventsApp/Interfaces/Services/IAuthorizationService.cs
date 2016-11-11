namespace BCEventsApp.Interfaces.Services
{
    public interface IAuthorizationService
    {
        bool CalendarOwner(string userId, int calendarId);

        bool EventOwner(string userId, int eventId);
    }
}