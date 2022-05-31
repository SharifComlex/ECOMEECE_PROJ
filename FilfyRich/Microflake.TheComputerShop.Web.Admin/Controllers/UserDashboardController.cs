using Microflake.Core.Application.Orders;
using Microflake.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.Web.Controllers
{
    public class UserDashboardController : BaseController
    {
        private readonly IOrderService _entityService;
        private readonly ApplicationDbContext _context;

        public UserDashboardController(ApplicationDbContext context, IOrderService entityService)
        {
            _entityService = entityService;
            _context = context;

        }
        // GET: UserDashboard
        public async Task<ActionResult> Index()
        {
            var list = await _entityService.List();
            if (Currency != null && Currency != "")
            {
                ViewBag.Currency = _context.Currencies.FirstOrDefault(x => x.Name == Currency).Currency_Rate;

            }
            else
            {
                ViewBag.Currency = 1;
            }
            return View(list.Data);
        }
    }
}