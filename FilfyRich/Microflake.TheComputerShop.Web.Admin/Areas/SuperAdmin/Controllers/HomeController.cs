using Microflake.TheComputerShop.Domain;
using Microflake.TheComputerShop.Persistence;
using Microflake.TheComputerShop.Web.Admin.Controllers;
using Microflake.TheComputerShop.Web.Admin.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microflake.TheComputerShop.Web.Admin.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class HomeController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        int currentyear = DateTime.UtcNow.Month;

        int Prevmonth = DateTime.UtcNow.Month - 1;
        

        // GET: SuperAdmin/Home
        public async Task<ActionResult> Index()
        {
           
                // ViewBag.ActiveRequests = await db.MaintencRequests
                // .Include(x => x.Customer)
                // .Include(t => t.Technitian)
                // .Where(x => x.Problemsolved == false

                // && x.RequestStatus != "Complleted"
                // && x.Amountpaid != true

                //).ToListAsync();
                ViewBag.ActiveProducts = db.Products.Where(x => x.Status== true).Count();
                ViewBag.InActiveProducts = db.Products.Where(x => x.Status == false).Count();


                ViewBag.Categories = await db.Categories.CountAsync();

            //var years = new List<int>();
            //for (int i = 2017; i <= DateTime.UtcNow.Year; i++)
            //{
            //    years.Add(i);
            //}

            var Products = await db.OrderDetals
                      .Include(x => x.Order)
                      .Include(x => x.Product)
                      .Where(x => x.Order.Status == "Completed").ToListAsync();
            //var Maintenance2 = maintenance.Where(x => x.CreatedAt.Year == currentyear).Sum(x => x.AdvanceAmount);

            //var tmaitenance = Maintenance + Maintenance2 ; 



            //ViewBag.Years = new SelectList(years.Select(x => new SelectListItem
            //{
            //    Text = x.ToString(),
            //    Value = x.ToString()
            //}), "Value", "Text", DateTime.UtcNow.Year);

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> getStats(int? year)
        {
            try
            {
                // for dashboard 
                var activeProducts = await db.Products.ToListAsync();

                if (activeProducts.Count() > 0)
                {   
                    ViewBag.ActiveProducts = activeProducts.Where(x => x.CreatedAt.Year == year && x.Status == true).Count();
                    ViewBag.InActiveProducts = activeProducts.Where(x => x.CreatedAt.Year == year && x.Status == false).Count();
                }

                var maintenancerequests = await db.Products.Where(x => x.Status == false ).ToListAsync();
                if (maintenancerequests.Count() > 0)
                {
                    maintenancerequests.Where(x => x.CreatedAt.Year == year);
                    ViewBag.SumofDuepayments = maintenancerequests.Sum(x => x.Price);
                }
                if (db.Products.Count() > 0)
                {
                    ViewBag.Expenses = db.Products.Where(x => x.CreatedAt.Year == year).Sum(x => x.Price);
                }
                var years = new List<int>();
                for (int i = 2017; i <= DateTime.UtcNow.Year; i++)
                {
                    years.Add(i);
                }

                var Products = await db.OrderDetals
                    .Include(x=>x.Order)
                    .Include(x => x.Product)
                    .Where(x => x.Order.Status == "Completed").ToListAsync();
                var ProductPurchasePrice = Products.Where(x => x.Order.CreatedAt.Year == year).Sum(x => x.Product.Price);

                var ProductSellPrice = Products.Where(x => x.Order.CreatedAt.Year == year).Sum(x => x.Product.SellPrice);

               //yearly Profit Loss
                ViewBag.ProfitLoss = ProductSellPrice - ProductPurchasePrice;

                var dbContext = new ApplicationDbContext();
                var UserId = User.Identity.GetUserId();

                var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == UserId);

               // var maintenanceRequests = await db.OrderDetals.Include(x => x.Product).Where(x => x.Order.Status == "Completed").ToListAsync();
                //var MaintenanceRequests = maintenanceRequests.Where(x => x.CreatedAt.Year == year).Sum(x => x.TotalBill);


               // var MaintenanceRequests = Maintenance + Maintenance2;



                //TempData["profit"] = expenses - MaintenanceRequests;

                //ViewBag.profitloss = expenses - MaintenanceRequests;

                var ghraphTotal = new List<double?>();
                for (int i = 1; i <= 12; i++)
                {
                    var sdate = $"{i}/1/{year}";
                    var start = DateTime.Parse(sdate);

                    DateTime nextMonth = new DateTime(start.Year, start.Month, 1).AddMonths(1);
                    var end = nextMonth.AddDays(-1);
                    // expenses to calculate monthly 

                    var PoductPrice = Products.Where(x => x.Order.CreatedAt>= start);
                    
                  var purchase = PoductPrice.Where(x => x.Order.CreatedAt <= end).Sum(x => x.Product.Price);

                    var PoductSellPrice = Products.Where(x => x.Order.CreatedAt >= start);

                    var Sell = PoductPrice.Where(x => x.Order.CreatedAt <= end).Sum(x => x.Product.SellPrice);

                    //var ProductsSellPrice = Products.Where(x => x.Order.OrderDate >= end.AddMonths(-1) && x.Order.OrderDate <= end).Sum(x => x.Product.SellPrice);

                    //var test =   db.OrderDetals
                    //       .Include(x => x.Order)
                    //  .Include(x => x.Product)
                    //      .Where(x => x.Order.CreatedAt >= start && x.Order.CreatedAt <= end).Sum(x => x.Product.SellPrice);

                    var Profitt = Sell - purchase;

                   ghraphTotal.Add(purchase);
                }

                return Json(new
                {
                    GhraphTotal = ghraphTotal,


                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Error = ex.Message
                }, JsonRequestBehavior.AllowGet);

            }
        }



        [HttpPost]
        public async Task<ActionResult> getpearkhours()
        {
            try
            {
                int year = 2020;
                using (var dbContext = new ApplicationDbContext())
                {
                    var date = DateTime.UtcNow;
                    var UserId = User.Identity.GetUserId();
                    var user = await dbContext
                        .Users
                        .FirstOrDefaultAsync(x => x.Id == UserId);

                   
                    //var endDate = startDate.AddHours(24);

                  
                   var Products = await db.OrderDetals
                    .Include(x => x.Order)
                    .Include(x => x.Product)
                    .Where(x => x.Order.Status == "Completed").ToListAsync();

                    var ghraphTotal = new List<double?>();
                    for (int i = 1; i <= 12; i++)
                    {
                        //var sdate = $"{i}/1/{year}";

                        //var start = DateTime.Parse(sdate);
                        var start = new DateTime(date.Year, i, 1, 0, 0, 0);
                        DateTime nextMonth = new DateTime(start.Year, start.Month, 1).AddMonths(1);
                        var end = nextMonth.AddDays(-1);
                        // expenses to calculate monthly 

                        var PoductPrice = Products
                            .Where(x => x.Order.CreatedAt >= start && x.Order.CreatedAt <= end)
                            .Sum(x => x.Product.Price);

                      //  var purchase = PoductPrice.Where(x => x.Order.CreatedAt <= end).Sum(x => x.Product.Price);

                        var PoductSellPrice = Products.Where(x => x.Order.CreatedAt >= start && x.Order.CreatedAt <= end).Sum(x => x.Product.SellPrice); 

                       // var Sell = PoductPrice.Where(x => 

                        //var ProductsSellPrice = Products.Where(x => x.Order.OrderDate >= end.AddMonths(-1) && x.Order.OrderDate <= end).Sum(x => x.Product.SellPrice);

                        //var test =   db.OrderDetals
                        //       .Include(x => x.Order)
                        //  .Include(x => x.Product)
                        //      .Where(x => x.Order.CreatedAt >= start && x.Order.CreatedAt <= end).Sum(x => x.Product.SellPrice);

                        var Profitt = PoductSellPrice - PoductPrice;

                        ghraphTotal.Add(Profitt);
                    }

                    //for (int i = 0; i < 48; i++)
                    //{
                    //var start = date.AddMinutes(-30);
                    //    TimeSpan ts = new TimeSpan(date.Ticks);
                    //    ghrappeakvalues.Add(new PeakViewModel
                    //    {
                    //        //Time = (date.Ticks - 621355968000000000) / 10000,
                    //        Time = ts.TotalMilliseconds - (1000 * 60 * 60 * 5),
                    //       Count = Appointments.Where(x => x.Order.OrderDate >= start && x.Order.OrderDate <= date).Count()
                    //    });
                    //    date = date.AddMinutes(30);
                    //}


                    return Json(new
                        {
                        GhraphTotal = ghraphTotal.ToArray()
                        });
                    }
                 
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Error = ex.Message
                });
            }
        }

    }
}