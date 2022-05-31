using Microflake.Core.Application.Currencies;
using Microflake.Core.ViewModel.Currency;
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
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyService _entityService;


        public CurrencyController(ICurrencyService entityService)
        {
            _entityService = entityService;
        }
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
        public async Task<ActionResult> Edit(List<EditCurrency> model)
        {
            if (model.Where(x => x.Base_Currency == true).Count() == 0 )
            {
                ModelState.AddModelError("Name",Language.Language.Please_Select_A_Base_Currency);
              
            }
            if (model.Where(x => x.Base_Currency == true).Count() > 0)
            {
                var basecurrent = model.Where(x => x.Base_Currency == true).FirstOrDefault().Currency_Rate;
                if (basecurrent != 1)
                {
                    ModelState.AddModelError("Name", "Base Currency Rate Should be 1 ");

                }

            }
          
            // var result = "";
            if (!ModelState.IsValid)
            {
                return ValidationErrors();
            }
            for(int i = 0; i < model.Count; i++)
               {
         var result = await _entityService.Update(model[i],  User.Identity.GetUserId());
            }
            return Json(new
            {
                status = true,
                //message = result.Message,
                //data = result.Data
            }, JsonRequestBehavior.AllowGet);
        }


    }
}