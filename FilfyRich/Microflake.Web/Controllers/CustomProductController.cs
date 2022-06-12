using Microflake.Core.Persistence;
using Microflake.Web.Areas.SuperAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.Web.Controllers
{
    public class CustomProductController : Controller
    {
        // GET: CustomProduct
        public async Task<ActionResult> Index()
        {
            using (var _context = new ApplicationDbContext()) {
                var caps = await _context.CustomColors.ToListAsync();
                var badges = await _context.CustomItems.ToListAsync();

                return View(new CustomItemModel
                {
                    Colors = caps,
                    Items = badges,
                });
            }
        }

        public async Task<ActionResult> ItemDetail(long ProductId, long ItemId)
        {
            using (var _context = new ApplicationDbContext())
            {
                var variation = await _context.CustomVariations
                           .FirstOrDefaultAsync(x=> x.CustomColorId == ProductId && x.CustomItemId == ItemId);

                return Json(variation);
            }
        }
    }
}