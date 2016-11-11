using BCEventsApp.Models;

namespace BCEventsApp.ApiModels
{
    public class AccountCreationRequest
    {
        public User User { get; set; }

        public string Password { get; set; }
    }
}