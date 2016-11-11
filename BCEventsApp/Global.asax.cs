using System.Web.Http;
using BCEventsApp.Application;

namespace BCEventsApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AppInitialize.InitializeCompositionRoot();
        }
    }
}
