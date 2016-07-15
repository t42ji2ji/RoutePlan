using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RoutePlan.Startup))]
namespace RoutePlan
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
