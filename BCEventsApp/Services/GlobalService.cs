using System;
using System.Collections.Generic;
using BCEventsApp.ApiModels;
using BCEventsApp.Cache;
using BCEventsApp.Exceptions;
using BCEventsApp.Interfaces.Services;
using BCEventsApp.Models;

namespace BCEventsApp.Services
{
    public class GlobalService : IGlobalService
    {
        private readonly MainCache mainCache;
        public GlobalService(MainCache mainCache)
        {
            this.mainCache = mainCache;
        }

        public List<SearchEvent> GetSearchEvents()
        {
            var searchEvents = new List<SearchEvent>();
            var events = mainCache.GetPublicEvents();
            foreach (var e in events)
            {
                if (e.Value.Public)
                {
                    var searchEvent = new SearchEvent()
                    {
                        Id = e.Value.Id,
                        Title = e.Value.Title,
                        OwnerName = e.Value.Owner.FirstName + " " + e.Value.Owner.LastName,
                    };
                    searchEvents.Add(searchEvent);
                }
            }
            return searchEvents;
        }

        public List<SearchEvent> GetSearchEvents(int calendarId)
        {
            var searchEvents = new List<SearchEvent>();
            var calendars = mainCache.GetCalendars();
            if (calendars.ContainsKey(calendarId))
            {
                var calendar = calendars[calendarId];
                var events = calendar.Events;
                foreach (var e in events)
                {
                    var searchEvent = new SearchEvent()
                    {
                        Id = e.Value.Id,
                        Title = e.Value.Title,
                        OwnerName = e.Value.Owner.FirstName + " " + e.Value.Owner.LastName
                    };
                    searchEvents.Add(searchEvent);
                }
            }
            return searchEvents;
        }

        public List<SearchCalendar> GetSearchCalendars()
        {
            var calendars = mainCache.GetCalendars();
            var searchCalendars = new List<SearchCalendar>();
            foreach (var calendar in calendars)
            {
                if (calendar.Value.Public)
                {
                    var searchCalendar = new SearchCalendar()
                    {
                        Id = calendar.Value.Id,
                        Title = calendar.Value.Title,
                        OwnerName = calendar.Value.Owner.FirstName + " " + calendar.Value.Owner.LastName
                    };
                    searchCalendars.Add(searchCalendar);
                }
            }
            return searchCalendars;
        }

        public User GetUserById(string userId)
        {
            var users = mainCache.GetUsers();
            if (users.ContainsKey(userId.ToLower()))
            {
                return users[userId.ToLower()];
            }
            throw CustomException.UserNotFoundException();
        }

        public Event GetEvent(int eventId)
        {
            var events = mainCache.GetEvents();
            return events.ContainsKey(eventId) ? events[eventId] : null;
        }

        public Calendar GetCalendar(int calendarId)
        {
            var publicCalendars = mainCache.GetPublicCalendars();
            return publicCalendars.ContainsKey(calendarId) ? publicCalendars[calendarId] : null;
        }

        public CalendarInfo GetCalendarInfo(int calendarId)
        {
            var publicCalendar = mainCache.GetPublicCalendars();
            if (publicCalendar.ContainsKey(calendarId))
            {
                var calendarInfo = new CalendarInfo()
                {
                    Title = publicCalendar[calendarId].Title,
                    UserId = publicCalendar[calendarId].Owner.Id
                };
                return calendarInfo;
            }
            return null;
        }

        public List<TimelineEntry> GetDailyTimeline()
        {
            const int limit = 14;
            var start = 0;
            var currentDay = DateTime.Today.Date;
            var timeline = mainCache.GetTimeline();
            if (timeline.Count == 0)
            {
                return null;
            }
            if (timeline.Keys[timeline.Count - 1] < currentDay)
            {
                return null;
            }
            if (currentDay > timeline.Keys[0])
            {
                for (var i = 0; i < timeline.Count; i++)
                {
                    if (timeline.ContainsKey(currentDay))
                    {
                        break;
                    }
                    currentDay = currentDay.AddDays(1);
                }
                start = timeline.IndexOfKey(currentDay);
            }
            return GetTimeLineSlice(start, limit);
        }

        public TimelineEvent GetTimelineEvent(int eventId)
        {
            var publicEvents = mainCache.GetPublicEvents();
            if (publicEvents.ContainsKey(eventId))
            {
                var e = publicEvents[eventId];
                var timelineEvent = new TimelineEvent()
                {
                    Id = e.Id,
                    Title = e.Title,
                    OwnerName = e.Owner.FirstName + " " + e.Owner.LastName,
                    AttendeesCount = e.Attendees.Count,
                    Start = e.Start
                };
                return timelineEvent;
            }
            return null;
        }

        public List<TimelineEntry> GetMoreTimelineEntries(DateTime lastEntry, int limit)
        {
            var timeline = mainCache.GetTimeline();
            if (timeline.ContainsKey(lastEntry))
            {
                var lastEntryIndex = timeline.IndexOfKey(lastEntry);
                return GetTimeLineSlice(lastEntryIndex + 1, limit);
            }
            return null;
        }
        
        private List<TimelineEntry> GetTimeLineSlice(int start, int limit)
        {
            var count = 0;
            var clientTimeline = new List<TimelineEntry>();
            var timeline = mainCache.GetTimeline();
            
            for (var i = start; i < timeline.Count; i++)
            {
                if (count > limit)
                {
                    break;
                }
                var eventList = new List<TimelineEvent>();
                foreach (var e in timeline.Values[i])
                {
                    var timelineEvent = new TimelineEvent()
                    {
                        Id = e.Value.Id,
                        Title = e.Value.Title,
                        OwnerName = e.Value.Owner.FirstName + " " + e.Value.Owner.LastName,
                        Start = e.Value.Start,
                        AttendeesCount = e.Value.Attendees.Count
                    };
                    eventList.Add(timelineEvent);
                }
                var timelineEntry = new TimelineEntry()
                {
                    Date = timeline.Keys[i],
                    EventList = eventList
                };
                clientTimeline.Add(timelineEntry);
                count ++;
            }
            return clientTimeline;
        }

        private sealed class InvertedComparer : IComparer<Int32>
        {
            public int Compare(int x, int y)
            {
                var result = y.CompareTo(x);
                return result == 0 ? 1 : result;
            }
        }

    }
}