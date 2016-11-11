using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BCEventsApp.ApiModels;
using BCEventsApp.Interfaces.Services;
using BCEventsApp.Models;

namespace BCEventsApp.Controllers
{
    public class UserController: ApiController
    {
        private readonly IUserService userService;
        private readonly IAuthorizationService authorizationService;
        private readonly IAuthenticationService authenticationService;

        public UserController(IUserService userService, IAuthorizationService authorizationService, IAuthenticationService authenticationService)
        {
            this.userService = userService;
            this.authorizationService = authorizationService;
            this.authenticationService = authenticationService;
        }
        
        [HttpPost]
        public async Task<List<Calendar>> GetAllCalendars(string token)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                return await userService.GetAllCalendars(userId);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task<Dictionary<int, Event>> GetAllSubscribedEvents(string token)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                return await userService.GetAllSubscribedEvents(userId);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task<int> GetPrimaryCalendarId(string token)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                return await userService.GetPrimaryCalendarId(userId);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task<int> CreateEvent(string token, [FromBody] Event e)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                if (authorizationService.CalendarOwner(userId, e.CalendarId))
                {
                    return await userService.CreateEvent(userId, e);
                }

                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task<int> CreateCalendar(string token, [FromBody] Calendar calendar)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                return await userService.CreateCalendar(userId, calendar);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task ModifyEvent(string token, [FromBody] Event e)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                if (authorizationService.EventOwner(userId, e.Id))
                {
                    await userService.ModifyEvent(e);
                    return;
                }

                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }
        
        [HttpPost]
        public async Task DeleteEvent(string token, [FromBody] CalendarEventId request)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                if (authorizationService.EventOwner(userId, request.EventId))
                {
                    await userService.DeleteEvent(request.CalendarId, request.EventId);
                    return;
                }
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task DeleteCalendar(string token, [FromBody] int calendarId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                if (authorizationService.CalendarOwner(userId, calendarId))
                {
                    await userService.DeleteCalendar(calendarId);
                    return;
                }
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task SubscribeEvent(string token, [FromBody] int eventId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                await userService.SubscribeEvent(eventId, userId);
                return;
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task UnsubscribeEvent(string token, [FromBody] int eventId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                await userService.UnsubscribeEvent(eventId, userId);
                return;
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task SubscribeCalendar(string token, [FromBody] int calendarId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                await userService.SubscribeCalendar(calendarId, userId);
                return;
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task UnSubscribeCalendar(string token, [FromBody] int calendarId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                await userService.UnsubscribeCalendar(calendarId, userId);
                return;
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task PublishCalendar(string token, [FromBody] int calendarId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                if (authorizationService.CalendarOwner(userId, calendarId))
                {
                    await userService.PublishCalendar(calendarId);
                    return;
                }
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task UnpublishCalendar(string token, [FromBody] int calendarId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                if (authorizationService.CalendarOwner(userId, calendarId))
                {
                    await userService.UnpublishCalendar(calendarId);
                    return;
                }
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }
    }
}