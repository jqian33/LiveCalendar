using System.Timers;
using System.Web.Http;
using BCEventsApp.Cache;
using BCEventsApp.Infrastructure;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using Timer = System.Timers.Timer;

namespace BCEventsApp.Application
{
    public class AppInitialize
    {
        private static MainCache MainCache;
        public static void InitializeCompositionRoot()
        {
            var container = new Container();
            CompositionRoot.Initialize(container);
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

            MainCache= container.GetInstance<MainCache>();
            
            MainCache.Initialize().Wait();
            
            var dailyTimer = new Timer(86400000);
            dailyTimer.Elapsed += new ElapsedEventHandler(DailyTimerElapsed);
        }

        static void DailyTimerElapsed(object sender, ElapsedEventArgs e)
        {
            MainCache.AdjustTimeline();
        }
    }
}