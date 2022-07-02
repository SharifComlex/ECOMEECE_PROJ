using Microflake.Core.Application.Categories;
using Microflake.Core.Application.Products;
using Microflake.Core.Domain;
using Microflake.Core.Persistence;
using Microflake.Core.Utilities;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.Products;
using Microflake.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Microflake.Web.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IProductService _entityService;

        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;
        public ShopController(ApplicationDbContext context ,IProductService entityService , ICategoryService categoryService)
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
        public async Task<ActionResult> CartItems()
        {
            CheckoutViewModel model = new CheckoutViewModel();
            var cart = new ShopService(HttpContext);
            var items = await cart.GetCartItemsAsync();
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
            //model.User = UserManager.FindById(User.Identity.GetUserId());
            return View(new ShoppingCartViewModel
            {
                Items = items,
                Total = CalcuateCart(items),
                User = UserManager.FindById(User.Identity.GetUserId()),
             

            });
        }

        public int ProductCount()
        {
            var cart = new ShopService(HttpContext);
            var items =  cart.GetCartItemsCount();
            return items;
        }
            public async Task<ActionResult> CartItemsModel()
        {
            CheckoutViewModel model = new CheckoutViewModel();
            var cart = new ShopService(HttpContext);
            var items = await cart.GetCartItemsAsync();
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
            //model.User = UserManager.FindById(User.Identity.GetUserId());
            return PartialView(new ShoppingCartViewModel
            {
                Items = items,
                Total = CalcuateCart(items),
                User = UserManager.FindById(User.Identity.GetUserId()),

            });
        }
        public async Task<ActionResult> Checkout()
        {
            CheckoutViewModel model = new CheckoutViewModel();
            var cart = new ShopService(HttpContext);
            var items = await cart.GetCartItemsAsync();
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
            //if (User.Identity.GetUserId() == null)
            //{
            //    ModelState.AddModelError("Name", "Please Login To Add Whishlist");
            //    return View(model);
            //}
            model.User = UserManager.FindById(User.Identity.GetUserId());
            var UserId = User.Identity.GetUserId();
            var user = _context.Users
                   

                    .Where(x => x.Id == UserId)
                    .FirstOrDefault();
            var checkOutmodel = new CheckoutViewModel
            {
                Items = items,
                Total = CalcuateCart(items)
            };
            return View(checkOutmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(CheckoutViewModel model)
        {
            var cart = new ShopService(HttpContext);
            var items = await cart.GetCartItemsAsync();

            if (!ModelState.IsValid)
            {
                model.Items = items;
                model.Total = CalcuateCart(items);
                model.User = UserManager.FindById(User.Identity.GetUserId());

                return View(model);
            }

            var result = await cart.CheckoutAsync(model, User.Identity.GetUserId());

            if (result > 0) {

                StripeConfiguration.ApiKey = "sk_test_51LA8LSA2T7xDNF0lMDd9YbipfEmze0PhS8NMUaHDoAKJESU0VhK2xoBxjBpz9AMGOtZbNkxNOvKIJZV4uiohbdnH00JVaU877v";

                try
                {
                    using (var _db = new ApplicationDbContext()) {

                        var orderDetail = _db.Orders.FirstOrDefault(x=> x.OrderId == result);

                        var options = new ChargeCreateOptions
                        {
                            Amount = (long)(orderDetail.Total * 100),
                            Currency = "GBP",
                            Description = model.FirstName + " " + model.LastName + ", OrderId =  " + result,
                            Source = model.stripeToken,
                        };

                        var service = new ChargeService();
                        Charge charge = service.Create(options);

                        if (charge.Captured)
                        {
                            orderDetail.TransactionId = charge.Id;
                            orderDetail.PaymentStatus = "Paid";

                            _db.Entry(orderDetail).State = EntityState.Modified;
                            if (_db.SaveChanges() > 0) {

                                await cart.EmptyCartItemsAsync();

                                Response.Cookies["ShoppingCart"].Expires = DateTime.UtcNow;
                                return RedirectToAction("index");
                            }
                        }

                        _db.Orders.Remove(orderDetail);
                        _db.SaveChanges();
                    }
                }
                catch (StripeException ex)
                {
                    ModelState.AddModelError("stripeToken", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("stripeToken", ex.Message);
                }

                model.Items = items;
                model.Total = CalcuateCart(items);
                model.User = UserManager.FindById(User.Identity.GetUserId());

                return View(model);
            }
            else {
                model.Items = items;
                model.Total = CalcuateCart(items);
                model.User = UserManager.FindById(User.Identity.GetUserId());

                return View(model);
            }
        }

        public async Task<ActionResult> Pay(long OrderId)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Pay(StripPayModel model)
        {
            if (ModelState.IsValid)
            {
                StripeConfiguration.ApiKey = "sk_test_wfSHnvBhrhUVB9pmt1b1DyKz00q1skyVH4";

                try
                {
                    long ChargeAmount = 0;

                    var options = new ChargeCreateOptions
                    {
                        Amount = (ChargeAmount * 100),
                        Currency = "usd",
                        Description = model.firstName + " " + model.middleName + " " + model.lastName,
                        Source = model.stripeToken,
                    };

                    var service = new ChargeService();
                    Charge charge = service.Create(options);

                }
                catch (StripeException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(model);
        }
        // GET: Shop
        public async Task<ActionResult> Index(int? subcategoryID,string searchTerm, int? Page, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy)
        {
            ListProduct model = new ListProduct();
            int recordSize = 9;
            Page = Page.HasValue ? Page.Value > 0 ? Page.Value : 1 : 1;
            model.Productlist =  _entityService.List(subcategoryID,searchTerm, Page.Value, recordSize, categoryID , minimumPrice, maximumPrice , sortBy);
            //ViewBag.Categorylist =await _categoryService.ToList();
            model.SubCategorylist = _context.SubCategories.ToList();
            model.Categorylist = _context.Categories.ToList();
            var TotalRecords =  _entityService.ProductCount(subcategoryID, searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
            model.Whislists = _context.Whislists.ToList();
            model.SearchTerm = searchTerm;
            model.SortBy = sortBy;
            model.CategoryID = categoryID;
            model.SubCategoryID = subcategoryID;
            model.MaximumPrice = _entityService.GetMaximumPrice();
            model.Pager= new Pager(TotalRecords, Page, recordSize);
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
            int recordSize = 9;
            Page = Page.HasValue ? Page.Value > 0 ? Page.Value : 1 : 1;
            model.Productlist = _entityService.List(subcategoryID,searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
            var TotalRecords = _entityService.ProductCount(subcategoryID, searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
            model.Pager = new Pager(TotalRecords, Page, recordSize);
            model.SortBy = sortBy;
            model.Whislists = _context.Whislists.ToList();
            model.CategoryID = categoryID;
            model.SubCategoryID = subcategoryID;
            model.MaximumPrice =Convert.ToDouble( maximumPrice);
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

        public async Task<ActionResult> AddToCart(int id)
        {
            var cart = new ShopService(HttpContext);

            await cart.AddAsync(id);

            return RedirectToAction("CartItems");
        }

        public async Task<ActionResult> CustomAddToCart(int productId, int frontChip,int backChip)
        {
            var cart = new ShopService(HttpContext);

            await cart.AddAsync(productId, frontChip, backChip);

            return RedirectToAction("CartItems");
        }

        public async Task<ActionResult> RemoveFromCart(int id)
        {
            var cart = new ShopService(HttpContext);

            await cart.RemoveAsync(id);

            return RedirectToAction("CartItems");
        }

        private static double CalcuateCart(IEnumerable<CartItem> items)
        {
            return items.Sum(item => (item.Product.SellPrice * item.Count));
        }
    }
}