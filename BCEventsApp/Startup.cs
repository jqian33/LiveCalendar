using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BCEventsApp.Startup))]
namespace BCEventsApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
