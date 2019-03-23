using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DreamLeague.Startup))]
namespace DreamLeague
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
