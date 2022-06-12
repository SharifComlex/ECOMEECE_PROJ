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
                model.CapBackChip = await _context.CustomVariations
                               .FirstOrDefaultAsync(x => x.CustomColorId == productId && x.CustomItemId == backChip);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(CustomCheckout model)
        {
            var _context = new ApplicationDbContext();

            if (!ModelState.IsValid)
            {
                model.Product = await _context.CustomVariations
                              .FirstOrDefaultAsync(x => x.CustomColorId == model.ProductId && x.CustomItemId == model.FrontChipId);

                if (model.FrontChipId != model.BackChipId)
                {
                    model.CapBackChip = await _context.CustomVariations
                                   .FirstOrDefaultAsync(x => x.CustomColorId == model.ProductId && x.CustomItemId == model.BackChipId);
                }

                return View(model);
            }

            model.Product = await _context.CustomVariations
                              .FirstOrDefaultAsync(x => x.CustomColorId == model.ProductId && x.CustomItemId == model.FrontChipId);

            if (model.FrontChipId != model.BackChipId)
            {
                model.CapBackChip = await _context.CustomVariations
                               .FirstOrDefaultAsync(x => x.CustomColorId == model.ProductId && x.CustomItemId == model.BackChipId);
            }

            var order = new CustomOrder()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                City = model.City,
                PostalCode = model.PostalCode,
                Country = model.Country,
                Phone = model.Phone,
                Email = model.Email,
                TransactionId = "",
                Status = "Pending",
                PaymentStatus = "UnPaid",
                OrderDate = DateTime.Now
            };

            var productPrice = model.Product.CustomColor.SellPrice;
            var frontChipPrice = model.Product.CustomItem.SellPrice;
            var backChipPrice = 0f;

            if (model.FrontChipId != model.BackChipId)
            {
                backChipPrice = model.CapBackChip.CustomItem.SellPrice;
            }
            else
            {
                backChipPrice = model.Product.CustomItem.SellPrice;
            }

            order.Total = (productPrice + frontChipPrice + backChipPrice) * model.Qty;

            _context.CustomOrders.Add(order);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                var orderItem = new CustomOrderDetail()
                {
                   CustomColorId = model.ProductId,
                   CustomItem1Id = model.FrontChipId,
                   CustomItem2Id = model.BackChipId,
                   Quantity = model.Qty,
                   OrderId = order.OrderId
                };

                _context.CustomOrderDetails.Add(orderItem);
                result = await _context.SaveChangesAsync();

                if (result > 0) {
                    StripeConfiguration.ApiKey = "sk_test_wfSHnvBhrhUVB9pmt1b1DyKz00q1skyVH4";

                    try
                    {
                        var options = new ChargeCreateOptions
                        {
                            Amount = (long)order.Total,
                            Currency = "GBP",
                            Description = model.FirstName + " " + model.LastName + ", OrderId =  " + result,
                            Source = model.stripeToken,
                        };

                        var service = new ChargeService();
                        Charge charge = service.Create(options);

                        if (charge.Captured)
                        {
                            var updateOrderItem = await _context.CustomOrderDetails.FirstOrDefaultAsync(x => x.OrderDetailId == orderItem.OrderDetailId);

                            updateOrderItem.TransactionId = charge.Id;
                            updateOrderItem.PaymentStatus = "Paid";

                            _context.Entry(updateOrderItem).State = EntityState.Modified;
                            
                            if (_context.SaveChanges() > 0)
                            {
                                return RedirectToAction("index");
                            }
                        }

                        _context.CustomOrders.Remove(order);
                        _context.SaveChanges();
                    }
                    catch (StripeException ex)
                    {
                        ModelState.AddModelError("stripeToken", ex.Message);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("stripeToken", ex.Message);
                    }
                }
            }

            return View(model);

        }
    }
}