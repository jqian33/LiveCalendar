using System.Threading.Tasks;
using BCEventsApp.ApiModels;
using BCEventsApp.Models;

namespace BCEventsApp.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<bool> Authenticate(string username, string password);

        UserIdToken VerifyTokenOnInitialize(string token);

        TokenPayload VerifyToken(string token);

        string IssueToken(string userId);
    }
}