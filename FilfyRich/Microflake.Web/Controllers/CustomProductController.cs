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
        public async Task<ActionResult> Index(long? Id)
        {
            if (!Id.HasValue) {
                return Redirect("/Shop");
            }

            var _context = new ApplicationDbContext();
            var caps = await _context.Products.Where(x=> x.Qty > 0 && x.Id == Id.Value).ToListAsync();

            if (caps.Count == 0) {
                return Redirect("/Shop");
            }

            var badgeIds = await _context.CustomVariations.Where(x => x.CapId == Id).Select(x=> x.BadgeId.Value).ToListAsync();
            var badges = await _context.Products.Where(x => x.Qty > 2 && badgeIds.Contains(x.Id)).ToListAsync();

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