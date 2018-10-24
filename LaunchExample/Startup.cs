using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Cyclr.LaunchExample.Startup))]
namespace Cyclr.LaunchExample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
