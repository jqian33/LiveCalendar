using System.Collections.Generic;
using System.Threading.Tasks;
using BCEventsApp.Cache;
using BCEventsApp.Interfaces.Database;
using BCEventsApp.Interfaces.Services;
using BCEventsApp.Models;

namespace BCEventsApp.Services
{
    public class UserService: IUserService
    {
        private readonly MainCache mainCache;

        private readonly IUserActions userActions;

        public UserService(MainCache mainCache, IUserActions userActions)
        {
            this.mainCache = mainCache;
            this.userActions = userActions;
        }

        public async Task<List<Calendar>> GetAllCalendars(string userId)
        {
            var calendarIdList = await userActions.GetAllCalendars(userId);
            var calendars = mainCache.GetUserCalendars(calendarIdList);
            return calendars;
        }

        public async Task<int> GetPrimaryCalendarId(string userId)
        {
            return await userActions.GetPrimaryCalendarId(userId);
        }

        public async Task<Dictionary<int, Event>> GetAllSubscribedEvents(string userId)
        {
            var eventIdList = await userActions.GetAllSubscribedEvents(userId);
            var events = mainCache.GetUserSubscribedEvents(eventIdList);
            return events;
        }

        public async Task<int> CreateEvent(string userId, Event e)
        {
            var user = mainCache.GetUsers()[userId];
            e.Start = e.Start.ToLocalTime();
            var eventId = await userActions.CreateEvent(user.Id, e.Title, e.Description, e.Location, e.Start, e.Duration, e.End, e.RRule, e.CalendarId);
            e.Id = eventId;
            e.Owner = user;
            mainCache.AddEvent(e);
            if (e.Public)
            {
                await userActions.PublishEvent(eventId);
            }
            return eventId;
        }

        public async Task ModifyEvent(Event e)
        {
            var events = mainCache.GetEvents();
            if (events.ContainsKey(e.Id))
            {
                var oldEvent = events[e.Id];
                e.Start = e.Start.ToLocalTime();
                if (e.Title != oldEvent.Title)
                {
                    await userActions.ModifyEventTitle(e.Id, e.Title);
                    oldEvent.Title = e.Title;
                }
                if (e.Description != oldEvent.Description)
                {
                    await userActions.ModifyEventDescription(e.Id, e.Description);
                    oldEvent.Description = e.Description;
                }
                if (e.Location != oldEvent.Location)
                {
                    await userActions.ModifyEventLocation(e.Id, e.Location);
                    oldEvent.Location = e.Location;
                }
                if (e.Start != oldEvent.Start)
                {
                    await userActions.ModifyEventStartTime(e.Id, e.Start);
                    var oldStart = oldEvent.Start;
                    oldEvent.Start = e.Start;
                    mainCache.MoveEventOnTimeline(oldStart.Date, oldEvent);
                }
                if (e.Duration != oldEvent.Duration)
                {
                    await userActions.ModifyEventDuration(e.Id, e.Duration);
                    oldEvent.Duration = e.Duration;
                }
                if (e.End != oldEvent.End)
                {
                    await userActions.ModifyEventEndTime(e.Id, e.End);
                    oldEvent.End = e.End;
                }
                if (e.RRule != oldEvent.RRule)
                {
                    await userActions.ModifyEventRRule(e.Id, e.RRule);
                    oldEvent.RRule = e.RRule;
                }
                if (e.Public && oldEvent.Public == false)
                {
                    await PublishEvent(e.Id);
                }
                if (e.Public == false && oldEvent.Public)
                {
                    await UnpublishEvent(e.Id);
                }
            }
        }

        public async Task PublishEvent(int eventId)
        {
            mainCache.PublishEvent(eventId);
            await userActions.PublishEvent(eventId);
        }

        public async Task UnpublishEvent(int eventId)
        {
            mainCache.UnpublishEvent(eventId);
            await userActions.UnpublishEvent(eventId);
        }

        public async Task<int> CreateCalendar(string userId, Calendar calendar)
        {
            var user = mainCache.GetUsers()[userId];
            var calendarId = await userActions.CreateCalendar(user.Id, calendar.Title, calendar.Description);
            calendar.Id = calendarId;
            calendar.Owner = user;
            calendar.Tags = new List<Tag>();
            calendar.Events = new Dictionary<int, Event>();
            mainCache.AddCalendar(calendar);
            if (calendar.Public)
            {
                await userActions.PublishCalendar(calendarId);
            }
            return calendarId;
        }

        public async Task DeleteEvent(int calendarId, int eventId)
        {
            await userActions.DeleteEvent(eventId);
            mainCache.DeleteEvent(calendarId, eventId);
        }

        public async Task SubscribeEvent(int eventId, string userId)
        {
            await userActions.SubscribeEvent(userId, eventId);
        }

        public async Task UnsubscribeEvent(int eventId, string userId)
        {
            await userActions.UnsubscribeEvent(userId, eventId);
        }

        public async Task SubscribeCalendar(int calendarId, string userId)
        {
            await userActions.SubscribeCalendar(userId, calendarId);
        }

        public async Task UnsubscribeCalendar(int calendarId, string userId)
        {
            await userActions.UnsubscribeCalendar(userId, calendarId);
        }

        public async Task UnpublishCalendar(int calendarId)
        {
            await userActions.UnpublishCalendar(calendarId);
            mainCache.UnpublishCalendar(calendarId);
        }

        public async Task PublishCalendar(int calendarId)
        {
            await userActions.PublishCalendar(calendarId);
            mainCache.PublishCalendar(calendarId);
        }

        public async Task DeleteCalendar(int calendarId)
        {
            await userActions.DeleteCalendar(calendarId);
            mainCache.DeleteCalendar(calendarId);
        }
    }
}