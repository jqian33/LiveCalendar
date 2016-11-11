using System.Collections.Generic;

namespace BCEventsApp.Hub
{
    public class CalendarHub: Microsoft.AspNet.SignalR.Hub
    {
        public void AddCalendar(int calendarId, string title, string ownerName, string ownerId)
        {
            Clients.Others.broadcastNewCalendar(calendarId, title, ownerName, ownerId);
        }

        public void DeleteCalendar(int calendarId, string ownerId, List<int> eventIdList )
        {
            Clients.Others.broadcastDeletedCalendar(calendarId, ownerId, eventIdList);
        }
    }
}