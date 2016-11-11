using System.Threading.Tasks;
using BCEventsApp.Models;

namespace BCEventsApp.Interfaces.Services
{
    public interface IAccountService
    {
        bool UserIdExists(string userId);

        Task CreateUser(User user, string hash);
    }
}
