using Microflake.Core.Domain;
using Microflake.Core.Persistence;
using Microflake.Web.Areas.SuperAdmin.Models;
using Microflake.Web.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.Web.Controllers
{
    public class CustomProductController : Controller
    {
        // GET: CustomProduct
        public async Task<ActionResult> Index()
        {
            var _context = new ApplicationDbContext();
            var caps = await _context.CustomColors.ToListAsync();
            var badges = await _context.CustomItems.ToListAsync();

            return View(new CustomItemModel
            {
                Colors = caps,
                Items = badges,
            });
        }

        public async Task<ActionResult> ItemDetail(long ProductId, long ItemId)
        {
            var _context = new ApplicationDbContext();
            var variation = await _context.CustomVariations
                           .FirstOrDefaultAsync(x => x.CustomColorId == ProductId && x.CustomItemId == ItemId);

            return Json(variation, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Checkout(long productId, long frontChip, long backChip, int qty)
        {
            var model = new CustomCheckout();
            model.ProductId = productId;
            model.FrontChipId = frontChip;
            model.BackChipId = backChip;
            model.Qty = qty;

            var _context = new ApplicationDbContext();

            model.Product = await _context.CustomVariations
                           .FirstOrDefaultAsync(x => x.CustomColorId == productId && x.CustomItemId == frontChip);

            if (frontChip != backChip) {
                model.BackChip = await _context.CustomVariations
                               .FirstOrDefaultAsync(x => x.CustomColorId == productId && x.CustomItemId == backChip);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(CustomCheckout model)
        {
            if (!ModelState.IsValid)
            {
                var _context = new ApplicationDbContext();

                model.Product = await _context.CustomVariations
                               .FirstOrDefaultAsync(x => x.CustomColorId == model.ProductId && x.CustomItemId == model.FrontChipId);

                if (model.FrontChipId != model.BackChipId)
                {
                    model.BackChip = await _context.CustomVariations
                                   .FirstOrDefaultAsync(x => x.CustomColorId == model.ProductId && x.CustomItemId == model.BackChipId);
                }

                return View(model);
            }

            var result = await cart.CheckoutAsync(model, User.Identity.GetUserId());

            if (result > 0)
            {

                StripeConfiguration.ApiKey = "sk_test_wfSHnvBhrhUVB9pmt1b1DyKz00q1skyVH4";

                try
                {
                    using (var _db = new ApplicationDbContext())
                    {

                        var orderDetail = _db.Orders.FirstOrDefault(x => x.OrderId == result);

                        var options = new ChargeCreateOptions
                        {
                            Amount = (long)orderDetail.Total,
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
            else
            {
                model.Items = items;
                model.Total = CalcuateCart(items);
                model.User = UserManager.FindById(User.Identity.GetUserId());

                return View(model);
            }
        }
    }
}