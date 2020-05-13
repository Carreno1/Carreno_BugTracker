using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Carreno_BugTracker.Startup))]
namespace Carreno_BugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
