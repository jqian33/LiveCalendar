using System.Threading.Tasks;
using BCEventsApp.Cache;
using BCEventsApp.Interfaces.Database;
using BCEventsApp.Interfaces.Services;

namespace BCEventsApp.Services
{
    public class EventService: IEventService
    {
        private readonly MainCache mainCache;

        private readonly IUserActions userActions;

        public EventService(MainCache mainCache, IUserActions userActions)
        {
            this.mainCache = mainCache;
            this.userActions = userActions;
        }

        public async Task AddAttendee(string userId, int eventId)
        {
            mainCache.AddAttendee(userId, eventId);
            mainCache.ChangeTimelineEventAttendeesCount(eventId);
            await userActions.AttendEvent(userId, eventId);
        }

        public async Task RemoveAttendee(string userId, int eventId)
        {
            mainCache.RemoveAttendee(userId, eventId);
            mainCache.ChangeTimelineEventAttendeesCount(eventId);
            await userActions.WithdrawEvent(userId, eventId);
        }
    }
}