using BCEventsApp.Cache;
using BCEventsApp.Database;
using BCEventsApp.Interfaces.Database;
using BCEventsApp.Interfaces.Services;
using BCEventsApp.Services;
using SimpleInjector;

namespace BCEventsApp.Infrastructure
{
    public class CompositionRoot
    {
        private static CompositionRoot Instance;

        private CompositionRoot(Container container)
        {
            container.RegisterSingleton<MainCache>();
            container.RegisterSingleton<IConnector, Connector>();
            container.RegisterSingleton<ICalendarActions, CalendarActions>();
            container.RegisterSingleton<IEventActions, EventActions>();
            container.RegisterSingleton<IGlobalActions, GlobalActions>();
            container.RegisterSingleton<IUserActions, UserActions>();

            container.RegisterSingleton<IUserService, UserService>();
            container.RegisterSingleton<IEventService, EventService>();
            container.RegisterSingleton<IGlobalService, GlobalService>();
            container.RegisterSingleton<IAuthorizationService, AuthorizationService>();
            container.RegisterSingleton<IAuthenticationService, AuthenticationService>();
            container.RegisterSingleton<IAccountService, AccountService>();
        }

        public static void Initialize(Container container)
        {
            if (Instance == null)
            {
                Instance = new CompositionRoot(container);
            }
        }
    }
}