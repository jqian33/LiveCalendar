using System;
using System.Threading.Tasks;
using BCEventsApp.ApiModels;
using BCEventsApp.Authentication;
using BCEventsApp.Cache;
using BCEventsApp.Interfaces.Database;
using BCEventsApp.Interfaces.Services;
using BCEventsApp.Models;

namespace BCEventsApp.Services
{
    public class AuthenticationService: IAuthenticationService
    {

        private readonly MainCache mainCache;

        private readonly IGlobalActions globalActions;

        public AuthenticationService(MainCache mainCache, IGlobalActions globalActions)
        {
            this.mainCache = mainCache;
            this.globalActions = globalActions;
        }
        public async Task<bool> Authenticate(string username, string password)
        {
            var passwordHash = CryptoFunctions.GetPasswordHash(password);
            return await globalActions.VerifyPasswordHash(username.ToLower(), passwordHash);
        }

        public UserIdToken VerifyTokenOnInitialize(string token)
        {
            var tokenPayload = CryptoFunctions.VerifyToken(token, mainCache.GetKey());
            if (tokenPayload != null)
            {
                var userIdToken = new UserIdToken()
                {
                    UserId = tokenPayload.UserId,
                    Token = token
                };
                if (tokenPayload.ExpireTime.Subtract(DateTime.Now) <= new TimeSpan(0, 0, 30, 0))
                {
                    userIdToken.Token = IssueToken(tokenPayload.UserId);
                    return userIdToken;
                }
                return userIdToken;
            }
            return null;
        }

        public TokenPayload VerifyToken(string token)
        {
            return CryptoFunctions.VerifyToken(token, mainCache.GetKey());
        }

        public string IssueToken(string userId)
        {
            return CryptoFunctions.GenerateToken(userId, mainCache.GetKey());
        }
    }
}