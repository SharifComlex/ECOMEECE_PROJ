using Microflake.Core.Application.CustomOrders;
using Microflake.Core.Persistence;
using Microflake.Core.ViewModel.CustomOrder;
using Microflake.Web.Controllers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.Web.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class CustomOrderController : BaseController
    {
        private readonly ICustomOrderService _entityService;

        private readonly ApplicationDbContext _context;

        public CustomOrderController(ApplicationDbContext context, ICustomOrderService entityService)
        {
            _entityService = entityService;
            _context = context;

        }
        // GET: SuperAdmin/CustomOrder
        public async Task<ActionResult> Index()
        {
            var list = await _entityService.List();

            return View(list.Data);
        }
        public async Task<ActionResult> GetList()
        {
            var result = await _entityService.List();

            return Json(new
            {
                status = result.Success,
                message = result.Message,
                data = result.Data
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var result = await _entityService.Edit(id);


            return PartialView(result.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditCustomOrder model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationErrors();
            }

            var result = await _entityService.Update(model, User.Identity.GetUserId());

            return Json(new
            {
                status = result.Success,
                message = result.Message,
                data = result.Data
            }, JsonRequestBehavior.AllowGet);
        }
    }
}