using Microflake.Core.Persistence;
using Microflake.Core.Utilities.Logger;
using Microflake.Core.Utilities.Response;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.Orders;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Application.Orders
{
   public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResponse _response;
        private readonly string _cartId;

        private readonly ILogger _logger;

        public OrderService(ApplicationDbContext context,
            ILogger logger, IResponse response)
        {
            _context = context;
            _response = response;

        }

        public  async Task<ServiceResponse<OrderDetail>> Detail(long id)
        {
            try
            {
                var entity = await _context
                    .Orders
                    .Include(x=>x.OrderDetails)
                    .FirstOrDefaultAsync(x => x.OrderId == id);

                if (entity != null)
                {
                    return _response.Create(true, "Fatched", new OrderDetail
                    {
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Address = entity.Address,
                        Country  = entity.Country,
                        City = entity.City,
                        PostalCode = entity.PostalCode,
                        Email = entity.Email,
                        Image = entity.OrderDetails.FirstOrDefault().Product.Image,
                        FrontBadge = entity.OrderDetails.FirstOrDefault().FrontBadge.Image,
                        BackBadge = entity.OrderDetails.FirstOrDefault().BackBadge.Image,
                        Status = entity.Status,
                        Total = entity.Total,
                        CreatedAt = entity.CreatedAt
                    });
                }

                return _response.Create(false, "Not Fatched", new OrderDetail());
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new OrderDetail());
            }
        }

        public async Task<ServiceResponse<EditOrder>> Edit(long id)
        {
            try
            {
                var entity = await _context
                    .Orders
                    .FirstOrDefaultAsync(x => x.OrderId == id);

                if (entity != null)
                {
                    return _response.Create(true, "Fatched", new EditOrder
                    {
                        Id = entity.OrderId,
                      
                        Status = entity.Status
                    });
                }

                return _response.Create(false, "Not Fatched", new EditOrder());
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new EditOrder());
            }
        }
        public async Task<ServiceResponse<List<ListOrder>>> List()
        {
            try
            {
                var list = await _context
                    .OrderDetals
                    .Select(x => new ListOrder
                    {
                        Id = x.OrderId,
                        FirstName = x.Order.FirstName,
                        LastName = x.Order.LastName,
                        PRoductTitle = x.Product.Name,
                        Quanty = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        Total = x.Product.Price * x.Quantity,
                        Status = x.Order.Status,
                       SellPrice=x.Product.SellPrice,
                        orderId = x.Order.OrderId,
                        OrderBy = x.Order.OrderBy,
                        Image=x.Product.Image

                    }).ToListAsync();

                return _response.Create(true, "All record has been fetched", list);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListOrder>());
            }
        }
        public async Task<ServiceResponse<ListOrder>> Update(EditOrder model, string userId)
        {
            try
            {
                var entity = _context.Orders.Find(model.Id);


                entity.CreatedAt = DateTime.Now;
                entity.CreatedById = userId;
                entity.ModifiedAt = DateTime.Now;
                entity.ModifiedById = userId;
                entity.Status = model.Status;
               
              

                _context.Entry(entity).State = EntityState.Modified;

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return _response.Create(true, "Record has been updated", await Find(entity.OrderId));
                }
                else
                {
                    return _response.Create(false, "Error while updating the record.", new ListOrder());
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new ListOrder());
            }
        }


        private async Task<ListOrder> Find(long Id)
        {
            var model = await _context
                    .OrderDetals
                    .Select(x => new ListOrder
                    {
                        Id = x.OrderId,
                        FirstName = x.Order.FirstName,
                        LastName = x.Order.LastName,
                        PRoductTitle = x.Product.Name,
                        Quanty = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        Total = x.Product.Price * x.Quantity,
                        Status = x.Order.Status,
                        SellPrice=x.Product.SellPrice,
                        orderId = x.Order.OrderId,
                        Image =x.Product.Image
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.orderId == Id);
            return model;
        }
        //private async Task<ListOrder> Find(long Id)
        //{
        //    var model = await _context
        //            .OrderDetals
        //            .Select(x => new ListOrder
        //            {
        //                Id = x.Product.Id,
        //                FirstName = x.Order.FirstName,
        //                LastName = x.Order.LastName,
        //                PRoductTitle = x.Product.Title,
        //                Quanty = x.Quantity,
        //                UnitPrice = x.UnitPrice,
        //                Total = x.Product.Price * x.Quantity,
        //                Status = x.Order.Status,

        //                orderId = x.Order.OrderId

        //            })
        //            .AsNoTracking()
        //            .FirstOrDefaultAsync(x => x.Id == Id);
        //    return model;
        //}

    }
}
