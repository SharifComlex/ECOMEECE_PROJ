using Microflake.Core.Persistence;
using Microflake.Web.Areas.SuperAdmin.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microflake.Web.Controllers
{
    public class CustomProductController : Controller
    {
        // GET: CustomProduct
        public async Task<ActionResult> Index()
        {
            var _context = new ApplicationDbContext();
            var products = await _context.Products.ToListAsync();
            var caps = products.Where(x => x.IsHasVariation).ToList();
            var badges = products.Where(x => x.IsVariationOverlay).ToList();

            return View(new CustomItemModel
            {
                Colors = caps,
                Items = badges,
            });
        }

        public async Task<ActionResult> ItemDetail(long ProductId, long ItemId)
        {
            var _context = new ApplicationDbContext();
            var variation = await _context.CustomVariations
                           .FirstOrDefaultAsync(x => x.CapId == ProductId && x.BadgeId == ItemId);

            return Json(variation, JsonRequestBehavior.AllowGet);
        }
    }
}