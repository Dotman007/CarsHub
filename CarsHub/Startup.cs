using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarsHub.Startup))]
namespace CarsHub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
