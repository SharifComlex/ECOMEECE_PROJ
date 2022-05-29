using Microflake.TheComputerShop.Application.Whislists;
using Microflake.TheComputerShop.Persistence;
using Microflake.TheComputerShop.Web.Admin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.TheComputerShop.Web.Admin.Controllers
{
    public class WhislistController : BaseController
    {
        private readonly IWhislistService _entityService;

        private readonly ApplicationDbContext _context;

        public WhislistController(ApplicationDbContext context, IWhislistService entityService)
        {
            _context = context;
            _entityService = entityService;

           

        }
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Whislist
        public async Task<ActionResult> Index()
        {
            var list = await _entityService.List();
            if (Currency != null && Currency != "")
            {
                ViewBag.Currency = _context.Currencies.FirstOrDefault(x => x.Name == Currency).Currency_Rate;
                ViewBag.CurrencySimbal = _context.Currencies.FirstOrDefault(x => x.Name == Currency).Simbal;
             
            }
            else
            {
                ViewBag.CurrencySimbal = _context.Currencies.FirstOrDefault(x => x.Base_Currency == true).Simbal;

                ViewBag.Currency = 1;
            }
            return View(list.Data);
        }

        public async Task<ActionResult> WhishListCount()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var userId = User.Identity.GetUserId();
            //var result = await _entityService.WhishListCount(user.Id);
          var result=   _context.Whislists.Where(x => x.CreatedById == userId).Count();
            return Json(new
            {
                
                data = result
            }, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        public async Task<ActionResult> Create(long id)
        {


            if (User.Identity.GetUserId() == null)
            {
                ModelState.AddModelError("Name", "Please Login To Add Whishlist");

            }

            if (!ModelState.IsValid)
            {
                return ValidationErrors();
            }
            var result = await _entityService.AddProduct(id, User.Identity.GetUserId());

            return Json(new
            {
                status = result.Success,
                message = result.Message,
                data = result.Data
            }, JsonRequestBehavior.AllowGet);

        }
        public async Task<ActionResult> remove(long id)
        {


            if (User.Identity.GetUserId() == null)
            {
                ModelState.AddModelError("Name", "Please Login To Remove Whishlist");

            }

            if (!ModelState.IsValid)
            {
                return ValidationErrors();
            }
            var result = await _entityService.AddProduct(id, User.Identity.GetUserId());

            return RedirectToAction("index");

        }


    }
}