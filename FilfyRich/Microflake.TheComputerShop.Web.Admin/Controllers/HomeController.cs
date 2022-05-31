using Microflake.Core.Application.Categories;
using Microflake.Core.Application.Products;
using Microflake.Core.Domain;
using Microflake.Core.Persistence;
using Microflake.Core.Utilities;
using Microflake.Core.ViewModel.Products;
using Microflake.Web.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IProductService _entityService;

        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;
        public HomeController(ApplicationDbContext context, IProductService entityService, ICategoryService categoryService)
        {
            _context = context;
            _entityService = entityService;
            _categoryService = categoryService;

        }

        public ActionResult Test()
        {
            return View();
        }
        public async Task<ActionResult> Index(string searchTerm, int? Page,int? subcategoryID, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy)
        {

            if (User.Identity.IsAuthenticated && User.IsInRole("SuperAdmin"))
            {
                return Redirect("/SuperAdmin");
            }


            ListProduct model = new ListProduct();
            int recordSize = 8;
            Page = Page.HasValue ? Page.Value > 0 ? Page.Value : 1 : 1;
            model.Productlist = _entityService.List(subcategoryID,searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
            //ViewBag.Categorylist =await _categoryService.ToList();
            model.SubCategorylist = _context.SubCategories.ToList();
            model.Categorylist = _context.Categories.ToList();
            var TotalRecords = _entityService.FeaturedProductCount(subcategoryID, searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
            model.Whislists = _context.Whislists.ToList();
            model.SearchTerm = searchTerm;
            model.SortBy = sortBy;
            model.CategoryID = categoryID;
            model.SubCategoryID = subcategoryID;
            model.MaximumPrice = _entityService.GetMaximumPrice();
            model.Pager = new Pager(TotalRecords, Page, recordSize);

            if (Currency != null && Currency != "")
            {
                model.Currency = _context.Currencies.FirstOrDefault(x => x.Name == Currency).Currency_Rate;
                model.CurrencySimbal = _context.Currencies.FirstOrDefault(x => x.Name == Currency).Simbal;
                model.MaximumPrice = model.Currency * model.MaximumPrice;

            }
            else
            {
                model.CurrencySimbal = _context.Currencies.FirstOrDefault(x => x.Base_Currency == true).Simbal;

                model.Currency = 1;
            }
            return View(model);
        }


        public async Task<ActionResult> FilterProduct(int? subcategoryID, string searchTerm, int? Page, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy)
        {
            ListProduct model = new ListProduct();
            int recordSize = 8;
            Page = Page.HasValue ? Page.Value > 0 ? Page.Value : 1 : 1;
            model.Productlist = _entityService.List(subcategoryID ,searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
            var TotalRecords = _entityService.FeaturedProductCount(subcategoryID, searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
            model.Pager = new Pager(TotalRecords, Page, recordSize);
            model.SortBy = sortBy;
            model.Whislists = _context.Whislists.ToList();
            model.CategoryID = categoryID;
            model.SubCategoryID = subcategoryID;
            model.MaximumPrice = Convert.ToDouble(maximumPrice);
            model.SearchTerm = searchTerm;
            if (Currency != null && Currency != "")
            {
                model.Currency = _context.Currencies.FirstOrDefault(x => x.Name == Currency).Currency_Rate;
                model.CurrencySimbal = _context.Currencies.FirstOrDefault(x => x.Name == Currency).Simbal;
              

            }
            else
            {
                model.CurrencySimbal = _context.Currencies.FirstOrDefault(x => x.Base_Currency == true).Simbal;

                model.Currency = 1;
            }
            return PartialView(model);
        }

        [Route("home/Language")]
        public ActionResult SetLanguage(string Name, string url)
        {
            if (Name != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Name);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Name);
            }

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = Name;
            Response.Cookies.Add(cookie);

            if (!string.IsNullOrEmpty(url))
            {
                return Redirect(url);
            }

            return RedirectToAction("Index");
        }
        // GET: Home


        public async Task<ActionResult> AboutUs()
        {
            
            return View();
        }

        public async Task<ActionResult> ContactUs()
        {

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ContactUs(ContactUs model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationErrors();
            }
            _context.ContactUss.Add(model);
            _context.SaveChanges();
            return Json(new
            {
                status = true,


            }, JsonRequestBehavior.AllowGet);
        }


    }
}