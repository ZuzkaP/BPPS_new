using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BPPS.Startup))]
namespace BPPS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
