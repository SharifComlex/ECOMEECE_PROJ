using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECap.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("SuperAdmin")) {
                return Redirect("/SuperAdmin/dashboard");
            }
            return View();
        }
    }
}