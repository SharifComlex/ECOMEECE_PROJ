using Microsoft.AspNetCore.Mvc;

namespace ECap.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
