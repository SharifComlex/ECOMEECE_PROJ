using System.Web.Mvc;

namespace Microflake.TheComputerShop.Web.Admin.Areas.SuperAdmin
{
    public class SuperAdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SuperAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapMvcAttributeRoutes();
            context.MapRoute(
                "SuperAdmin_default",
                "SuperAdmin/{controller}/{action}/{id}",

          new { controller = "Home", action = "Index", id = UrlParameter.Optional }


            );
        }
    }
}