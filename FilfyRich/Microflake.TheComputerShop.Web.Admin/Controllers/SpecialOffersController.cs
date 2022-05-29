using Microflake.TheComputerShop.Application.Categories;
using Microflake.TheComputerShop.Application.Products;
using Microflake.TheComputerShop.Persistence;
using Microflake.TheComputerShop.Utilities;
using Microflake.TheComputerShop.ViewModel.Products;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.TheComputerShop.Web.Admin.Controllers
{
    public class SpecialOffersController : BaseController
    {
        private readonly IProductService _entityService;

        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;
        public SpecialOffersController(ApplicationDbContext context, IProductService entityService, ICategoryService categoryService)
        {
            _context = context;
            _entityService = entityService;
            _categoryService = categoryService;
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
        // GET: SpecialOffers
        public async Task<ActionResult> Index(int? subcategoryID,string searchTerm, int? Page, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy)
        {
            ListProduct model = new ListProduct();
            int recordSize = 8;
            Page = Page.HasValue ? Page.Value > 0 ? Page.Value : 1 : 1;
            model.Productlist = _entityService.List(subcategoryID,searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
            model.SubCategorylist = _context.SubCategories.ToList();
            model.Categorylist = _context.Categories.ToList();
            var TotalRecords = _entityService.ProductCountOfDeal(subcategoryID, searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);

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
            model.Productlist = _entityService.List(subcategoryID,searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
            var TotalRecords = _entityService.ProductCountOfDeal(subcategoryID, searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
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

    }
}