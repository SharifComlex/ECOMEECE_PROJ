using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Microflake.Web.Startup))]

namespace Microflake.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
