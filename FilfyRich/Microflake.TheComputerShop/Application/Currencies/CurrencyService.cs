using Microflake.TheComputerShop.Persistence;
using Microflake.TheComputerShop.Utilities.Logger;
using Microflake.TheComputerShop.Utilities.Response;
using Microflake.TheComputerShop.ViewModel;
using Microflake.TheComputerShop.ViewModel.Currency;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.Application.Currencies
{
   public class CurrencyService : ICurrencyService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResponse _response;
        private readonly string _cartId;

        private readonly ILogger _logger;

        public CurrencyService(ApplicationDbContext context,
            ILogger logger, IResponse response)
        {
            _context = context;
            _response = response;

        }

        public async Task<ServiceResponse<EditCurrency>> Edit(long id)
        {
            try
            {
                var entity = await _context
                    .Currencies
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (entity != null)
                {
                    return _response.Create(true, "Fatched", new EditCurrency
                    {
                        Base_Currency = entity.Base_Currency,

                        Currency_Rate = entity.Currency_Rate
                    });
                }

                return _response.Create(false, "Not Fatched", new EditCurrency());
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new EditCurrency());
            }
        }
        public async Task<ServiceResponse<List<ListCurrency>>> List()
        {
            try
            {
                var list = await _context
                    .Currencies
                    .Select(x => new ListCurrency
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Currency_Rate = x.Currency_Rate,
                        Base_Currency = x.Base_Currency


                    }).ToListAsync();

                return _response.Create(true, "All record has been fetched", list);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListCurrency>());
            }
        }
        public async Task<ServiceResponse<ListCurrency>> Update(EditCurrency model, string userId)
        {
            try
            {
                var entity = _context.Currencies.Find(model.Id);


                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedById = userId;
                entity.ModifiedAt = DateTime.UtcNow;
                entity.ModifiedById = userId;
                entity.Currency_Rate = model.Currency_Rate;
                entity.Base_Currency = model.Base_Currency;


                _context.Entry(entity).State = EntityState.Modified;

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return _response.Create(true, "Record has been updated", await Find(entity.Id));
                }
                else
                {
                    return _response.Create(false, "Error while updating the record.", new ListCurrency());
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new ListCurrency());
            }
        }


        private async Task<ListCurrency> Find(long Id)
        {
            var model = await _context
                    .Currencies
                    .Select(x => new ListCurrency
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Currency_Rate = x.Currency_Rate,
                        Base_Currency = x.Base_Currency

                       

                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Id);
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
