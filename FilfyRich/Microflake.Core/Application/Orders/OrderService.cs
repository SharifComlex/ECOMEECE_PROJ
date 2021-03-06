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

        public  async Task<ServiceResponse<ListOrder>> Detail(long id)
        {
            try
            {
                var entity = await _context
                   .Orders
                   .Select(x => new ListOrder
                   {
                       Id = x.OrderId,
                       FirstName = x.FirstName,
                       LastName = x.LastName,
                       Email = x.Email,
                       Address = x.Address,
                       ShippingCharges = x.ShippingCharges,
                       Quanty = x.OrderDetails.Select(q => q.Quantity).Sum(),
                       Total = x.OrderDetails.Select(q => q.Quantity * q.UnitPrice).Sum() + x.ShippingCharges,
                       Status = x.Status,
                       CreatedAt = x.CreatedAt,
                       Items = x.OrderDetails.Select(o => new OrderDetail
                       {
                           OrderId = o.OrderId,
                           Name = o.Product.Name,
                           CapImage = o.Product.Image,
                           FrontBadge = o.FrontBadge.Image,
                           BackBadge = o.BackBadge.Image,
                           IsCustom = o.IsCustom,
                           Quantity = o.Quantity,
                           UnitPrice = o.UnitPrice
                       })
                       .ToList()
                   }).FirstOrDefaultAsync(x=> x.Id == id);


                if (entity != null)
                {
                    return _response.Create(true, "Fatched", entity);
                }

                return _response.Create(false, "Not Fatched", new ListOrder());
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new ListOrder());
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
                    .Orders
                    .Select(x => new ListOrder
                    {
                        Id = x.OrderId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Quanty = x.OrderDetails.Select(q=> q.Quantity).Sum(),
                        Total = x.OrderDetails.Select(q => q.Quantity * q.UnitPrice).Sum() + x.ShippingCharges,
                        Status = x.Status,
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
                        Quanty = x.Quantity,
                        Total = x.Product.Price * x.Quantity,
                        Status = x.Order.Status,
                        orderId = x.Order.OrderId,
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
