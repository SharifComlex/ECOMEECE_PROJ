using Microflake.Core.Application.Orders;
using Microflake.Core.ViewModel.Orders;
using Microflake.Core.ViewModel.Products;
using Microflake.Web.Controllers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microflake.Web.Models;
using Microflake.Core.Persistence;

namespace Microflake.Web.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _entityService;

        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context, IOrderService entityService)
        {
            _entityService = entityService;
            _context = context;

        }
        // GET: SuperAdmin/Products
        // GET: SuperAdmin/AttributeGroups
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
        public async Task<ActionResult> Detail(int id)
        {
            var result = await _entityService.Detail(id);


            return PartialView(result.Data);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditOrder model)
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