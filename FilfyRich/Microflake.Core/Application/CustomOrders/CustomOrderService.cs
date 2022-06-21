using Microflake.Core.Persistence;
using Microflake.Core.Utilities.Logger;
using Microflake.Core.Utilities.Response;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.CustomOrder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Application.CustomOrders
{
  public  class CustomOrderService : ICustomOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResponse _response;
        private readonly string _cartId;

        private readonly ILogger _logger;

        public CustomOrderService(ApplicationDbContext context,
            ILogger logger, IResponse response)
        {
            _context = context;
            _response = response;

        }

        public async Task<ServiceResponse<EditCustomOrder>> Edit(long id)
        {
            try
            {
                var entity = await _context
                    .CustomOrders
                    .FirstOrDefaultAsync(x => x.OrderId == id);

                if (entity != null)
                {
                    return _response.Create(true, "Fatched", new EditCustomOrder
                    {
                        Id = entity.OrderId,

                        Status = entity.Status
                    });
                }

                return _response.Create(false, "Not Fatched", new EditCustomOrder());
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new EditCustomOrder());
            }
        }
        public async Task<ServiceResponse<List<ListCustomOrder>>> List()
        {
            try
            {
                var list = await _context
                     .CustomOrderDetails
                    .Select(x => new ListCustomOrder
                    {
                        Id = x.OrderId,
                        FirstName = x.Order.FirstName,
                        LastName = x.Order.LastName,
                        PRoductTitle = x.CustomItem1.Name,
                        Quanty = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        Total = (x.CustomColor.SellPrice) + (x.CustomItem1.SellPrice) + (x.CustomItem2.SellPrice),
                        Status = x.Order.Status,
                        orderId = x.Order.OrderId,
                        CustomColorId = x.CustomColor.Image,

                        CustomItem1Id = x.CustomItem1.Image,

                        CustomItem2Id = x.CustomItem2.Image,
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return _response.Create(true, "All record has been fetched", list);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListCustomOrder>());
            }
        }
        public async Task<ServiceResponse<ListCustomOrder>> Update(EditCustomOrder model, string userId)
        {
            try
            {
                var entity = _context.CustomOrders.Find(model.Id);


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
                    return _response.Create(false, "Error while updating the record.", new ListCustomOrder());
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new ListCustomOrder());
            }
        }


        private async Task<ListCustomOrder> Find(long Id)
        {
            var model = await _context
                    .CustomOrderDetails
                    .Select(x => new ListCustomOrder
                    {
                        Id = x.OrderId,
                        FirstName = x.Order.FirstName,
                        LastName = x.Order.LastName,
                        PRoductTitle = x.CustomItem1.Name,
                        Quanty = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        Total = (x.CustomColor.SellPrice ) + (x.CustomItem1.SellPrice) + (x.CustomItem2.SellPrice),
                        Status = x.Order.Status,
                        orderId = x.Order.OrderId,
                        CustomColorId = x.CustomColor.Image,

                        CustomItem1Id = x.CustomItem1.Image,

                        CustomItem2Id = x.CustomColor.Image,
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
