using Microflake.Core.Application.ContactUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.Web.Areas.SuperAdmin.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IContactUsService _entityService;


        public ContactUsController(IContactUsService entityService)
        {
            _entityService = entityService;
        }
        // GET: SuperAdmin/ContactUs
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


    }
}