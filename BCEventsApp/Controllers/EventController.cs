using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BCEventsApp.Interfaces.Services;

namespace BCEventsApp.Controllers
{
    public class EventController : ApiController
    {
        private readonly IEventService eventService;
        private readonly IAuthenticationService authenticationService;

        public EventController(IEventService eventService, IAuthenticationService authenticationService)
        {
            this.eventService = eventService;
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task AddAttendee(string token, [FromBody] int eventId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                await eventService.AddAttendee(userId, eventId);
                return;
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task RemoveAttendee(string token, [FromBody] int eventId)
        {
            var tokenPayload = authenticationService.VerifyToken(token);
            if (tokenPayload != null)
            {
                var userId = tokenPayload.UserId;
                await eventService.RemoveAttendee(userId, eventId);
                return;
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

    }
}
