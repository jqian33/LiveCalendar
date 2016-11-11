using System.Collections.Generic;
using System.Threading.Tasks;
using BCEventsApp.Authentication;
using BCEventsApp.Cache;
using BCEventsApp.Interfaces.Database;
using BCEventsApp.Interfaces.Services;
using BCEventsApp.Models;

namespace BCEventsApp.Services
{
    public class AccountService: IAccountService
    {
        private readonly MainCache mainCache;

        private readonly IGlobalActions globalActions;

        private readonly IUserActions userActions;

        public AccountService(MainCache mainCache, IGlobalActions globalActions, IUserActions userActions)
        {
            this.mainCache = mainCache;
            this.globalActions = globalActions;
            this.userActions = userActions;
        }

        public bool UserIdExists(string userId)
        {
            var users = mainCache.GetUsers();
            return users.ContainsKey(userId.ToLower());
        }

        public async Task CreateUser(User user, string password)
        {
            mainCache.AddUser(user);
            string passwordHash = CryptoFunctions.GetPasswordHash(password);
            await globalActions.AddUser(user, passwordHash);
            var primaryCalendarId = await userActions.GetPrimaryCalendarId(user.Id);
            var calendar = await globalActions.GetCalendarById(primaryCalendarId);
            calendar.Public = false;
            calendar.Owner = user;
            calendar.Tags = new List<Tag>();
            calendar.Events = new Dictionary<int, Event>();
            mainCache.AddCalendar(calendar);
        }
    }
}