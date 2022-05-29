using System.Linq;
using System.Web.Mvc;
using System.Globalization;
using System.Threading;
using System;
using System.Collections.Generic;

using System.Web;
using Microflake.TheComputerShop.Persistence;

namespace Microflake.TheComputerShop.Web.Admin.Controllers
{
    
    public abstract class BaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        protected bool _language = false;
        protected string  Currency = "";
        public BaseController()
        {
           
            try
            {
                var cookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

                var Currencycookie = System.Web.HttpContext.Current.Request.Cookies["Currency"];

                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "ar")
                    {
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ar");
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar");

                        _language = true;
                        Thread.CurrentThread.CurrentCulture.DateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
                    }
                    else
                    {
                        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    }
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                }
                //HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

                //if (cookie != null && cookie.Value != null)
                //{
                //    if (cookie.Value == "ar")
                //    {
                //        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ar");
                //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar");
                //    }
                //}
                if (Currencycookie != null && Currencycookie.Value != null)
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    Currency = Currencycookie.Value;
                }
            }
            catch (Exception)
            {
            }
        }

        public ActionResult ValidationErrors()
        {
            return Json(new
            {
                status = false,
                validation = false,
                message = "provide all required inputs.",
                errors = ModelState.Where(ms => ms.Value.Errors.Any())
                                        .Select(x => new {
                                            x.Key,
                                            Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                                        })
            }, JsonRequestBehavior.AllowGet);
        }

    }
}