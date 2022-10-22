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
using System.Net.Mail;
using System.Net;
using System.Web.UI.WebControls;
using System.Net.Http;

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
            var cart = new ShopService(HttpContext);
            var items = await cart.GetCartItemsAsync();
           
            return View(new ShoppingCartViewModel
            {
                Items = items,
                Total = CalcuateCart(items),
                User = UserManager.FindById(User.Identity.GetUserId())
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
            var cart = new ShopService(HttpContext);
            var items = await cart.GetCartItemsAsync();

            return PartialView(new ShoppingCartViewModel
            {
                Items = items,
                Total = CalcuateCart(items),
                User = UserManager.FindById(User.Identity.GetUserId())
            });
        }
        public async Task<ActionResult> Checkout()
        {
            CheckoutViewModel model = new CheckoutViewModel();
            var cart = new ShopService(HttpContext);
            var items = await cart.GetCartItemsAsync();

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

            if (!await cart.GetCartItemsCountAsync())
            {
                return Redirect("/shop/CartItems");
            }

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
                    var _db = new ApplicationDbContext();
                    var orderDetail = _db.Orders.FirstOrDefault(x => x.OrderId == result);

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

                        if (_db.SaveChanges() > 0)
                        {

                            await cart.EmptyCartItemsAsync();
                            await SendEmail(orderDetail.Email, orderDetail.OrderId);
                            Response.Cookies["ShoppingCart"].Expires = DateTime.UtcNow;
                            TempData["Checkout"] = "Created";
                            return RedirectToAction("index");
                        }
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

                var _db1 = new ApplicationDbContext();
                var orderDetail1 = _db1.Orders.FirstOrDefault(x => x.OrderId == result);
                _db1.Orders.Remove(orderDetail1);
                _db1.SaveChanges();

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
                var cart = new ShopService(HttpContext);

                if (!await cart.GetCartItemsCountAsync()) {
                    return Redirect("/shop/Checkout");
                }

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
            using (var db = new ApplicationDbContext()) {
                ViewBag.created = TempData["Checkout"];
                ViewBag.outofstock = TempData["outofstock"];

                ListProduct model = new ListProduct();
                int recordSize = 9;
                Page = Page.HasValue ? Page.Value > 0 ? Page.Value : 1 : 1;
                model.Productlist = _entityService.List(subcategoryID, searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
                //ViewBag.Categorylist =await _categoryService.ToList();
                model.SubCategorylist = await db.SubCategories.ToListAsync();
                model.Categorylist = await db.Categories.ToListAsync();
                var TotalRecords = _entityService.ProductCount(subcategoryID, searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
                model.Whislists = await _context.Whislists.ToListAsync();
                model.SearchTerm = searchTerm;
                model.SortBy = sortBy;
                model.CategoryID = categoryID;
                model.SubCategoryID = subcategoryID;
                model.MaximumPrice = _entityService.GetMaximumPrice();
                model.Pager = new Pager(TotalRecords, Page, recordSize);

                return View(model);
            }
        }
       
        public async Task<ActionResult> FilterProduct(int? subcategoryID, string searchTerm, int? Page, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy)
        {
            using (var db = new ApplicationDbContext()) {
                ListProduct model = new ListProduct();

                int recordSize = 9;
                Page = Page.HasValue ? Page.Value > 0 ? Page.Value : 1 : 1;
                model.Productlist = _entityService.List(subcategoryID, searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
                var TotalRecords = _entityService.ProductCount(subcategoryID, searchTerm, Page.Value, recordSize, categoryID, minimumPrice, maximumPrice, sortBy);
                model.Pager = new Pager(TotalRecords, Page, recordSize);
                model.SortBy = sortBy;
                model.Whislists = await _context.Whislists.ToListAsync();
                model.CategoryID = categoryID;
                model.SubCategoryID = subcategoryID;
                model.MaximumPrice = Convert.ToDouble(maximumPrice);
                model.SearchTerm = searchTerm;

                return PartialView(model);
            }
        }

        public async Task<ActionResult> AddToCart(int id)
        {
            var cart = new ShopService(HttpContext);

            if (!await cart.IsItemOutOfStockAsync(id))
            {
                TempData["outofstock"] = "outofstock";
                return RedirectToAction("index");
            }

            await cart.AddAsync(id);

            return RedirectToAction("CartItems");
        }

        public async Task<ActionResult> CustomAddToCart(int? productId, int? frontChip,int? backChip)
        {
            if (!productId.HasValue || !frontChip.HasValue || !backChip.HasValue)
            {
                return Redirect("/Shop");
            }

            if (productId.Value == 0 || frontChip.Value == 0 || backChip.Value == 0) {
                return Redirect("/Shop");
            }

            var cart = new ShopService(HttpContext);

            if (!await cart.IsItemOutOfStockAsync(productId.Value))
            {
                TempData["outofstock"] = "outofstock";
                return RedirectToAction("index");
            }

            if (!await cart.IsItemOutOfStockAsync(frontChip.Value))
            {
                TempData["outofstock"] = "outofstock";
                return RedirectToAction("index");
            }

            if (!await cart.IsItemOutOfStockAsync(backChip.Value))
            {
                TempData["outofstock"] = "outofstock";
                return RedirectToAction("index");
            }

            if (frontChip.Value == backChip.Value) {
                if (await cart.GetItemQtyAsync(frontChip.Value)< 2) {
                    TempData["outofstock"] = "outofstock";
                    return RedirectToAction("index");
                }
            }

            await cart.AddAsync(productId.Value, frontChip.Value, backChip.Value);

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
            return items.Where(x=> x.Product.Qty > 0).Sum(item => (item.Product.SellPrice * item.Count));
        }

        public async Task<ActionResult> orderReceipt(int id)
        {
            using (var db = new ApplicationDbContext()) {
                var order = await db.Orders.FirstOrDefaultAsync(x => x.OrderId == id);

                if (order == null) {
                    return Content("");
                }

                var orderItems = await db.OrderDetals.Include(x=> x.Product)
                                 .Include(x=> x.FrontBadge)
                                 .Include(x=> x.BackBadge)
                                 .Where(x=> x.OrderId == order.OrderId).ToListAsync();

                return PartialView(new ReceiptModel()
                {
                    Order = order,
                    Items = orderItems
                });
            }
        }

        private async Task<int> SendEmail(string email,long Id)
        {
            try
            {
                SmtpClient client = new SmtpClient("relay-hosting.secureserver.net", 465);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("info@filfyrich.com", "Goldboots1");

                MailMessage message = new MailMessage("info@filfyrich.com", email);
                message.Subject = "Order Receipt";

                using (var httpClient = new HttpClient()) {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Web");
                    message.Body = await httpClient.GetStringAsync("http://filfyrich.co.uk/shop/orderReceipt/" + Id);
                }
                   

                client.Send(message);
            }
            catch (Exception ex)
            {
            }

            return 1;
        }
    }
}