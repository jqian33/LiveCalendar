using System;
using System.Collections.Generic;
using BCEventsApp.ApiModels;
using BCEventsApp.Models;

namespace BCEventsApp.Interfaces.Services
{
    public interface IGlobalService
    {
        List<SearchEvent> GetSearchEvents();

        List<SearchEvent> GetSearchEvents(int calendarId);

        List<SearchCalendar> GetSearchCalendars();
        
        User GetUserById(string userId);

        Event GetEvent(int eventId);

        Calendar GetCalendar(int calendarId);

        CalendarInfo GetCalendarInfo(int calendarId);

        TimelineEvent GetTimelineEvent(int eventId);

        List<TimelineEntry> GetDailyTimeline();

        List<TimelineEntry> GetMoreTimelineEntries(DateTime lastEntry, int limit);

    }
}