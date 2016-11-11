using System.Threading.Tasks;
using System.Web.Http;
using BCEventsApp.ApiModels;
using BCEventsApp.Interfaces.Services;

namespace BCEventsApp.Controllers
{
    public class AuthenticationController: ApiController
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<bool> Authenticate([FromBody] UserLogin userLogin)
        {
            return await authenticationService.Authenticate(userLogin.UserId, userLogin.Password);
        }

        [HttpPost]
        public UserIdToken VerifyToken([FromBody] string token)
        {
            return authenticationService.VerifyTokenOnInitialize(token);
        }

        [HttpPost]
        public string IssueToken([FromBody] string userId)
        {
            return authenticationService.IssueToken(userId);
        }
    }
}