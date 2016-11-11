using System.Threading.Tasks;
using System.Web.Http;
using BCEventsApp.ApiModels;
using BCEventsApp.Interfaces.Services;

namespace BCEventsApp.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        public bool UserIdExists([FromBody] string userId)
        {
            return accountService.UserIdExists(userId);
        }

        [HttpPost]
        public async Task CreateAccount([FromBody] AccountCreationRequest request)
        {
            await accountService.CreateUser(request.User, request.Password);
        }
    }
}
