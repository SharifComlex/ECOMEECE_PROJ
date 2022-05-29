using Microflake.TheComputerShop.Domain;
using Microflake.TheComputerShop.Persistence;
using Microflake.TheComputerShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microflake.TheComputerShop.Application.Products
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

        public async Task<int> RemoveAsync(int productId)
        {
            var cartItem = await _db.CartItems
                .SingleOrDefaultAsync(c => c.ProductId == productId && c.CartId == _cartId);

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
            return await _db.CartItems.Include("Product")
                .Where(c => c.CartId == _cartId).ToArrayAsync();
        }

        public async Task<object> CheckoutAsync(CheckoutViewModel model,string userId)
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
                    TransactionId = "asd",
                    Status = "Pending",
                    OrderDate = DateTime.Now,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    OrderBy = userId
               

            };

                foreach (var item in items)
                {
                    var detail = new OrderDetals()
                    {
                        ProductId = item.ProductId,
                        UnitPrice = item.Product.Price,
                        Quantity = item.Count
                    };

                    order.Total += (item.Product.Price * item.Count);

                    order.OrderDetails.Add(detail);
                }

                // TODO: authorize payment
                // TODO: assign the transactionid

                _db.Orders.Add(order);
                return _db.SaveChanges();
              
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
