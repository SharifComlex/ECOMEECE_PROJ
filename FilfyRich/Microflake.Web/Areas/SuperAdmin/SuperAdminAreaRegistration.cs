using System.Web.Mvc;

namespace Microflake.Web.Areas.SuperAdmin
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
                "SuperAdmin_route",
                "SuperAdmin/{controller}/{action}/{id}",

          new { controller = "Home", action = "Index", id = UrlParameter.Optional }


            );
        }
    }
}