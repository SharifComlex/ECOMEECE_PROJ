using Microflake.Core.Domain;
using Microflake.Core.Persistence;
using Microflake.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microflake.Core.Application.Products
{
    public class ShopService
    {
        private readonly ApplicationDbContext _db;
        private readonly string _cartId;

        public ShopService(HttpContextBase context)
            : this(context, new ApplicationDbContext())
        {
        }

        public ShopService(HttpContextBase httpContext, ApplicationDbContext cbContext)
        {
            _db = cbContext;
            _cartId = GetCartId(httpContext);
        }

        #region Singleton
        public static ShopService Instance
        {
            get
            {
                if (instance == null) instance = new ShopService();

                return instance;
            }
        }
        private static ShopService instance { get; set; }

        private ShopService()
        {
        }

        #endregion

        //public int SaveOrder(Order order)
        //{
        //    using (var context = new CBContext())
        //    {
        //        context.Orders.Add(order);
        //        return context.SaveChanges();
        //    }
        //}

        public async Task AddAsync(int productId)
        {
            var product = await _db.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                // TODO: throw exception or do something
                return;
            }

            var cartItem = await _db.CartItems
                .SingleOrDefaultAsync(c => c.ProductId == productId && c.CartId == _cartId);

            if (cartItem != null)
            {
                cartItem.Count++;
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    CartId = _cartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                _db.CartItems.Add(cartItem);
            }

            await _db.SaveChangesAsync();
        }

        public async Task AddAsync(int productId, int FrontChip, int BackChip)
        {
            var product = await _db.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                // TODO: throw exception or do something
                return;
            }

            var cartItem = await _db.CartItems
                .SingleOrDefaultAsync(c => c.ProductId == productId
                && c.BackChipId == BackChip
                && c.FrontChipId == FrontChip
                && c.CartId == _cartId);

            if (cartItem != null)
            {
                cartItem.Count++;
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    CartId = _cartId,
                    Count = 1,
                    FrontChipId = FrontChip,
                    BackChipId = BackChip,
                    DateCreated = DateTime.Now
                };

                _db.CartItems.Add(cartItem);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(int productId)
        {
            var cartItem = await _db.CartItems
                .SingleOrDefaultAsync(c => c.CartItemId == productId && c.CartId == _cartId);

            var itemCount = 0;

            if (cartItem == null)
            {
                return itemCount;
            }

            if (cartItem.Count > 1)
            {
                cartItem.Count--;
                itemCount = cartItem.Count;
            }
            else
            {
                _db.CartItems.Remove(cartItem);
            }

            await _db.SaveChangesAsync();

            return itemCount;
        }

        public int GetCartItemsCount()
        {
            return  _db.CartItems.Include("Product")
                .Where(c => c.CartId == _cartId).Count();
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync()
        {
            return await _db.CartItems.Include(x=> x.Product)
                .Include(x => x.FrontChip)
                .Include(x => x.BackChip)
                .Where(c => c.CartId == _cartId).ToListAsync();
        }

        public async Task<long> EmptyCartItemsAsync()
        {
            var items = await _db.CartItems.Include(x => x.Product)
                .Where(c => c.CartId == _cartId).ToListAsync();

            _db.CartItems.RemoveRange(items);
            return await _db.SaveChangesAsync();
        }

        public async Task<long> CheckoutAsync(CheckoutViewModel model,string userId)
        {
            try
            {
                var items = await GetCartItemsAsync();
                var order = new Order()
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
                    OrderDate = DateTime.Now,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    OrderBy = userId
            };

                foreach (var item in items)
                {
                    order.Total += (item.Product.SellPrice * item.Count);
                }

                _db.Orders.Add(order);
                var result = _db.SaveChanges();

                if (result > 0) {
                    foreach (var item in items)
                    {
                        var detail = new OrderDetals()
                        {
                            ProductId = (long)item.ProductId,
                            UnitPrice = item.Product.SellPrice,
                            Quantity = item.Count,
                            OrderId = order.OrderId,
                            FrontBadgeId = item.FrontChipId,
                            BackBadgeId = item.BackChipId
                        };

                        if (detail.FrontBadgeId != null && detail.BackBadgeId != null) {
                            if (detail.FrontBadgeId.HasValue && detail.BackBadgeId.HasValue)
                            {
                                detail.IsCustom = true;
                            }
                        }

                        _db.OrderDetals.Add(detail);
                    }

                    _db.SaveChanges();
                }

                return order.OrderId;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private string GetCartId(HttpContextBase http)
        {
            var cookie = http.Request.Cookies.Get("ShoppingCart");
            var cartId = string.Empty;

            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookie = new HttpCookie("ShoppingCart");
                cartId = Guid.NewGuid().ToString();

                cookie.Value = cartId;
                cookie.Expires = DateTime.Now.AddDays(7);

                http.Response.Cookies.Add(cookie);
            }
            else
            {
                cartId = cookie.Value;
            }

            return cartId;
        }

    }
}
