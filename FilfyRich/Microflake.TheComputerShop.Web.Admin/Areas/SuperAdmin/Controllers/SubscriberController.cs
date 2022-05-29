using Microflake.TheComputerShop.Application.Subscribers;
using Microflake.TheComputerShop.Web.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.TheComputerShop.Web.Admin.Areas.SuperAdmin.Controllers
{
    public class SubscriberController : BaseController
    {
        private readonly ISubscribersService _entityService;


        public SubscriberController(ISubscribersService entityService)
        {
            _entityService = entityService;
        }
        // GET: SuperAdmin/Subscriber
        public async Task<ActionResult> Index()
        {
            var list = await _entityService.ToList();
            return View(list.Data);
        }
        public async Task<ActionResult> GetList()
        {
            var result = await _entityService.ToList();

            return Json(new
            {
                status = result.Success,
                message = result.Message,
                data = result.Data
            }, JsonRequestBehavior.AllowGet);
        }
    }
}