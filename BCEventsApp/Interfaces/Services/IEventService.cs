using System.Threading.Tasks;

namespace BCEventsApp.Interfaces.Services
{
    public interface IEventService
    {
        Task AddAttendee(string userId, int eventId);

        Task RemoveAttendee(string userId, int eventId);
    }
}
