using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCEventsApp.Authentication;
using BCEventsApp.Interfaces.Database;
using BCEventsApp.Models;

namespace BCEventsApp.Cache
{
    public class MainCache
    {
        private Dictionary<int, Calendar> calendars;

        private Dictionary<string, User> users;

        private Dictionary<int, Event> events;

        private Dictionary<int, Tag> tags;

        private Dictionary<int, Calendar> publicCalendars;

        private Dictionary<int, Event> publicEvents;

        private SortedList<DateTime, SortedList<int, Event>> timeline;

        private string key;

        private readonly IEventActions eventActions;

        private readonly IGlobalActions globalActions;

        private readonly ICalendarActions calendarActions;

        public MainCache(IEventActions eventActions, IGlobalActions globalActions, ICalendarActions calendarActions )
        {
            this.eventActions = eventActions;
            this.globalActions = globalActions;
            this.calendarActions = calendarActions;
        }

        public async Task Initialize()
        {
            calendars = await globalActions.GetAllCalendars();
            users = await globalActions.GetAllUsers();
            events = await globalActions.GetAllEvents();
            tags = await globalActions.GetAllTags();

            await PopulateEventOwner();
            await PopulateEventTags();
            await PopulateEventAttendees();
            await PopulateCalendarOwner();
            await PopulateCalendarTags();
            await PopulateCalendarEvents();
            await InitializePublicCalendars();
            await InitializePublicEvents();
            InitializeTimeline();
            key = CryptoFunctions.GenerateKey();
        }

        // Populate owner property for all events
        private async Task PopulateEventOwner()
        {
            foreach (var e in events)
            {
                var id = await eventActions.GetOwnerId(e.Key);
                if (users.ContainsKey(id))
                {
                    var user = users[id];
                    e.Value.Owner = user;
                }
            }
        }

        public async Task InitializePublicCalendars()
        {
            publicCalendars = new Dictionary<int, Calendar>();
            var idList = await globalActions.GetPublishedCalendarIds();
            foreach (var calendar in calendars)
            {
                if (idList.Contains(calendar.Key))
                {
                    calendar.Value.Public = true;
                    publicCalendars.Add(calendar.Key, calendar.Value);
                }
                else
                {
                    calendar.Value.Public = false;
                }
            }
        }
        
        private async Task InitializePublicEvents()
        {
            publicEvents = new Dictionary<int, Event>();
            var idList = await globalActions.GetPublishedEventIds();
            foreach (var e in events)
            {
                if (idList.Contains(e.Key) || publicCalendars.ContainsKey(e.Value.CalendarId))
                {
                    e.Value.Public = true;
                    publicEvents.Add(e.Key, e.Value);
                }
                else
                {
                    e.Value.Public = false;
                }
            }
        }

        private sealed class InvertedComparer : IComparer<Int32>
        {
            public int Compare(int x, int y)
            {
                var result = y.CompareTo(x);
                return result == 0 ? 1 : result;
            }
        }

        private void InitializeTimeline()
        {
            timeline = new SortedList<DateTime, SortedList<int, Event>>();
            var sortedPublicEvents = publicEvents.GroupBy(x => x.Value.Start.Date).ToDictionary(x => x.Key, x => x.ToList());
            foreach (var entry in sortedPublicEvents)
            {
                if (entry.Key >= DateTime.Today.Date)
                {
                    var eventList = new SortedList<int, Event>(new InvertedComparer());
                    foreach (var e in entry.Value)
                    {
                        eventList.Add(e.Value.Attendees.Count, e.Value);
                    }
                    timeline.Add(entry.Key, eventList);
                }
            }
        }

        public void AdjustTimeline()
        {
            foreach (var entry in timeline)
            {
                if (entry.Key < DateTime.Today.Date)
                {
                    timeline.Remove(entry.Key);
                }
                else
                {
                    break;
                }
            }
        }

        // Populate tags property for all events
        private async Task PopulateEventTags()
        {
            foreach (var e in events)
            {
                e.Value.Tags = new List<Tag>();
                var ids = await eventActions.GetTagIds(e.Key);
                foreach (var id in ids)
                {
                    if (tags.ContainsKey(id))
                    {
                        var tag = tags[id];
                        e.Value.Tags.Add(tag);
                    }
                }
            }
        }

        // Populate attendees property for all events
        private async Task PopulateEventAttendees()
        {
            foreach (var e in events)
            {
                e.Value.Attendees = new List<User>();
                var ids = await eventActions.GetAttendeeIds(e.Key);
                foreach (var id in ids)
                {
                    if (users.ContainsKey(id))
                    {
                        var user = users[id];
                        e.Value.Attendees.Add(user);
                    }
                }
            }
        }

        // Populate owner property for all calendars
        private async Task PopulateCalendarOwner()
        {
            foreach (var calendar in calendars)
            {
                var id = await calendarActions.GetOwnerId(calendar.Key);
                if (users.ContainsKey(id))
                {
                    var user = users[id];
                    calendar.Value.Owner = user;
                }
            }
        }

        // Populate tags property for all calendars
        private async Task PopulateCalendarTags()
        {
            foreach (var calendar in calendars)
            {
                calendar.Value.Tags = new List<Tag>();
                var ids = await calendarActions.GetTagIds(calendar.Key);
                foreach (var id in ids)
                {
                    if (tags.ContainsKey(id))
                    {
                        var tag = tags[id];
                        calendar.Value.Tags.Add(tag);
                    }
                }
            }
        }

        // Populate events property for all calendars
        private async Task PopulateCalendarEvents()
        {
            foreach (var calendar in calendars)
            {
                calendar.Value.Events = new Dictionary<int, Event>();
                var ids = await calendarActions.GetEventIds(calendar.Key);
                foreach (var id in ids)
                {
                    if (events.ContainsKey(id))
                    {
                        var e = events[id];
                        e.CalendarId = calendar.Key;
                        calendar.Value.Events.Add(id, e);
                    }
                }
            }
        }

        public void UnpublishCalendar(int calendarId)
        {
            if (calendars.ContainsKey(calendarId))
            {
                var calendar = calendars[calendarId];
                calendar.Public = false;
                foreach (var e in calendar.Events)
                {
                    e.Value.Public = false;
                    if (publicEvents.ContainsKey(e.Key))
                    {
                        publicEvents.Remove(e.Key);
                    }
                }
                if (publicCalendars.ContainsKey(calendarId))
                {
                    publicCalendars.Remove(calendarId);
                }
            }
        }

        public void PublishCalendar(int calendarId)
        {
            if (calendars.ContainsKey(calendarId))
            {
                var calendar = calendars[calendarId];
                calendar.Public = true;
                foreach (var e in calendar.Events)
                {
                    e.Value.Public = true;
                    if (publicEvents.ContainsKey(e.Key) == false)
                    {
                        publicEvents.Add(e.Key, e.Value);
                    }
                }
                publicCalendars.Add(calendarId, calendar);
            }
        }

        public void PublishEvent(int eventId)
        {
            if (events.ContainsKey(eventId))
            {
                var e = events[eventId];
                e.Public = true;
                publicEvents.Add(eventId, e);
            }
        }

        public void UnpublishEvent(int eventId)
        {
            if (events.ContainsKey(eventId))
            {
                events[eventId].Public = false;
            }
            if (publicEvents.ContainsKey(eventId))
            {
                publicEvents.Remove(eventId);
            }
        }

        public void AddAttendee(string userId, int eventId)
        {
            if (events.ContainsKey(eventId) && users.ContainsKey(userId))
            {
                var e = events[eventId];
                var user = users[userId];
                e.Attendees.Add(user);
            }
        }

        public void RemoveAttendee(string userId, int eventId)
        {
            if (events.ContainsKey(eventId) && users.ContainsKey(userId))
            {
                var e = events[eventId];
                var user = users[userId];
                e.Attendees.Remove(user);
            }
        }

        public Dictionary<int, Calendar> GetCalendars()
        {
            return calendars;
        }

        public Dictionary<int, Event> GetEvents()
        {
            return events;
        }

        public Dictionary<int, Calendar> GetPublicCalendars()
        {
            return publicCalendars;
        }

        public Dictionary<int, Event> GetPublicEvents()
        {
            return publicEvents;
        }

        public Dictionary<string, User> GetUsers()
        {
            return users;
        }

        public SortedList<DateTime, SortedList<int, Event>> GetTimeline()
        {
            return timeline;
        } 

        public string GetKey()
        {
            return key;
        }

        public void AddUser(User user)
        {
            users.Add(user.Id.ToLower(), user);
        }

        public void AddEvent(Event e)
        {
            events.Add(e.Id, e);
            if (e.Public)
            {
                publicEvents.Add(e.Id, e);
            }
            calendars[e.CalendarId].Events.Add(e.Id, e);

            if (timeline.ContainsKey(e.Start.Date))
            {
                timeline[e.Start.Date].Add(e.Attendees.Count, e);
            }
            else
            {
                var dayEntry = new SortedList<int, Event>(new InvertedComparer());
                dayEntry.Add(e.Attendees.Count, e);
                timeline.Add(e.Start.Date, dayEntry);
            }
        }

        public void DeleteEvent(int calendarId, int eventId)
        {
            if (calendars.ContainsKey(calendarId))
            {
                if (calendars[calendarId].Events.ContainsKey(eventId))
                {
                    calendars[calendarId].Events.Remove(eventId);
                }
            }
            if (events.ContainsKey(eventId))
            {
                events.Remove(eventId);
            }
            if (publicEvents.ContainsKey(eventId))
            {
                var eventStart = publicEvents[eventId].Start.Date;
                if (timeline.ContainsKey(eventStart))
                {
                    var dayEntry = timeline[eventStart];
                    for(var i = 0; i<dayEntry.Count; i++)
                    {
                        if (dayEntry.Values[i].Id == eventId)
                        {
                            dayEntry.RemoveAt(i);
                            break;
                        }
                    }
                    if (dayEntry.Count == 0)
                    {
                        timeline.Remove(eventStart);
                    }
                }
                publicEvents.Remove(eventId);
            }
        }

        public void DeleteCalendar(int calendarId)
        {
            if (calendars.ContainsKey(calendarId))
            {
                calendars.Remove(calendarId);
            }
            if (publicCalendars.ContainsKey(calendarId))
            {
                publicCalendars.Remove(calendarId);
            }
        }

        public void AddCalendar(Calendar calendar)
        {
            calendars.Add(calendar.Id, calendar);
            if (calendar.Public)
            {
                publicCalendars.Add(calendar.Id, calendar);
            }
        }

        public Dictionary<int, Event> GetUserSubscribedEvents(List<int> eventIdList)
        {
            var subscribedEvents = new Dictionary<int, Event>();
            foreach (var eventId in eventIdList)
            {
                if (events.ContainsKey(eventId))
                {
                    subscribedEvents.Add(eventId, events[eventId]);
                }
            }
            return subscribedEvents;
        }

        public void ChangeTimelineEventAttendeesCount(int eventId)
        {
            if (events.ContainsKey(eventId))
            {
                var eventStartDate = events[eventId].Start.Date;
                if (timeline.ContainsKey(eventStartDate))
                {
                    var eventList = timeline[eventStartDate];
                    for (var i = 0; i<eventList.Count; i++)
                    {
                        if (eventList.Values[i].Id == eventId)
                        {
                            var e = eventList.Values[i];
                            eventList.RemoveAt(i);
                            eventList.Add(e.Attendees.Count, e);
                            break;
                        }
                    }
                }
            }
        }

        public void MoveEventOnTimeline(DateTime oldStartDate, Event e)
        {
            if (timeline.ContainsKey(oldStartDate))
            {
                var dayEntry = timeline[oldStartDate];
                for (var i = 0; i < dayEntry.Count; i++)
                {
                    if (dayEntry.Values[i].Id == e.Id)
                    {
                        dayEntry.RemoveAt(i);
                        break;
                    }
                }
                if (dayEntry.Count == 0)
                {
                    timeline.Remove(oldStartDate);
                }
            }
            if (e.Start.Date > DateTime.Today.Date)
            {
                if (timeline.ContainsKey(e.Start.Date))
                {
                    timeline[e.Start.Date].Add(e.Attendees.Count, e);
                }
                else
                {
                    var dayEntry = new SortedList<int, Event>(new InvertedComparer());
                    dayEntry.Add(e.Attendees.Count, e);
                    timeline.Add(e.Start.Date, dayEntry);
                }
            }
        }

        public List<Calendar> GetUserCalendars(List<int> calendarIdList)
        {
            var userCalendars = new List<Calendar>();
            foreach (var id in calendarIdList)
            {
                if (calendars.ContainsKey(id))
                {
                    userCalendars.Add(calendars[id]);
                }
            }

            return userCalendars;
        } 

    }
}