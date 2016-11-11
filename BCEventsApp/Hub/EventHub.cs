using System;
using BCEventsApp.ApiModels;
using BCEventsApp.Models;

namespace BCEventsApp.Hub
{
    public class EventHub : Microsoft.AspNet.SignalR.Hub
    {
        public void AddEvent(SearchEvent searchEvent, int calendarId)
        {
            Clients.Others.broadcastNewEvent(searchEvent, calendarId);
        }

        public void DeleteEvent(int eventId, int calendarId, DateTime eventStart)
        {
            Clients.Others.broadcastDeletedEvent(eventId, calendarId, eventStart);
        }

        public void EditEvent(int eventId)
        {
            Clients.Others.broadcastEditedEvent(eventId);
        }

        public void ChangeStartDate(DateTime oldStartDate, int eventId)
        {
            Clients.Others.broadcastEditedEventStartDate(oldStartDate, eventId);
        }

        public void AddAttendee(int eventId, User user, DateTime eventStart)
        {
            Clients.All.broadcastAddedAttendee(eventId, user, eventStart);
        }

        public void RemoveAttendee(int eventId, string userId, DateTime eventStart)
        {
            Clients.All.broadcastDeletedAttendee(eventId, userId, eventStart);
        }
    }
}