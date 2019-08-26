using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(doorserve.Startup))]
namespace doorserve
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
