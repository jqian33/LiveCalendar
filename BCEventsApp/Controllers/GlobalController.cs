using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using BCEventsApp.ApiModels;
using BCEventsApp.Interfaces.Services;
using BCEventsApp.Models;

namespace BCEventsApp.Controllers
{
    public class GlobalController : ApiController
    {

        // Dependencies
        private readonly IGlobalService globalService;
        private readonly IAuthenticationService authenticationService;
        
        public GlobalController(IGlobalService globalService, IAuthenticationService authenticationService)
        {
            this.globalService = globalService;
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        public List<SearchEvent> GetSearchEvents(string token)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                return globalService.GetSearchEvents();
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public List<SearchEvent> GetSearchEventsByCalendarId(string token, [FromBody] int calendarId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                return globalService.GetSearchEvents(calendarId);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }
        
        [HttpPost]
        public List<SearchCalendar> GetSearchCalendars(string token)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                return globalService.GetSearchCalendars();
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public User GetUser(string token)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                return globalService.GetUserById(userId);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }
        
        [HttpPost]
        public Event GetEvent(string token, [FromBody] int eventId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                return globalService.GetEvent(eventId);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public Calendar GetCalendar(string token, [FromBody] int calendarId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                return globalService.GetCalendar(calendarId);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public CalendarInfo GetCalendarInfo(string token, [FromBody] int calendarId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                return globalService.GetCalendarInfo(calendarId);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public TimelineEvent GetTimelineEvent(string token, [FromBody] int eventId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                return globalService.GetTimelineEvent(eventId);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public List<TimelineEntry> GetTimeline(string token)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                return globalService.GetDailyTimeline();
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public List<TimelineEntry> GetMoreTimelineEntries(string token, [FromBody] DateTime lastEntryDate)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                return globalService.GetMoreTimelineEntries(lastEntryDate, 7);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }
    }
}
