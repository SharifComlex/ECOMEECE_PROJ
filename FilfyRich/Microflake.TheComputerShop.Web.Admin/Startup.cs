using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Microflake.TheComputerShop.Web.Admin.Startup))]

namespace Microflake.TheComputerShop.Web.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
