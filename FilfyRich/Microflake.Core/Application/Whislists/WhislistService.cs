using Microflake.Core.Domain;
using Microflake.Core.Persistence;
using Microflake.Core.Utilities.Logger;
using Microflake.Core.Utilities.Response;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.WhishList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Application.Whislists
{
  public  class WhislistService  :IWhislistService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResponse _response;
        private readonly string _cartId;

        private readonly ILogger _logger;

        public WhislistService(ApplicationDbContext context,
            ILogger logger, IResponse response)
        {
            _context = context;
            _response = response;

        }
        public async Task<ServiceResponse<List<ListWhishlist>>> List()
        {
            try
            {
                var list = await _context
                    .Whislists
                    .Include(x=>x.Product)
                    .Select(x => new ListWhishlist
                    {
                        Id = x.Id,
                        productId = x.Product.Id,
                        productName = x.Product.Name,
                        productPrice = x.Product.SellPrice,
                        Image = x.Product.Image,
                        Image1 = x.Product.Image1,
                        UserID = x.UserID,
                        CreatedById = x.CreatedById
                       

                    }).ToListAsync();

                return _response.Create(true, "All record has been fetched", list);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListWhishlist>());
            }
        }

        public async Task<ServiceResponse<long>> AddProduct(long id, string userId)
        {
            try
            {
                var entity1 = _context.Whislists.FirstOrDefault(x => x.ProductId == id);
                if (entity1 != null)
                {
                    _context.Whislists.Remove(entity1);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return _response.Create(true, "Record has been removed.", 0L);
                    }
                    else
                    {
                        return _response.Create(false, "Error while updating the record.", entity1.Id);
                    }
                }
                else
                {
                    var entity = new Whislist();

                    entity.ProductId = id;
                    /*-------------- CommonEntities Entries ---------------*/

                    entity.CreatedAt = DateTime.UtcNow;
                    entity.ModifiedAt = DateTime.UtcNow;
                    entity.CreatedById = userId;
                    entity.ModifiedById = userId;

                    /*-----------------------------------------------------*/

                    entity.Status = true;
                    _context.Whislists.Add(entity);
                    var entityResult = await _context.SaveChangesAsync();
                    return _response.Create(entityResult, 1, 0L);
                }
               
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, 0L);
            }
        }

        public async Task<ListWhishlist> FindSignleListEntity(long Id)
        {
            return await _context
                     .Whislists
                     .Select(x => new ListWhishlist
                     {
                         Id = x.Id,
                        
                     }).SingleOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<ServiceResponse<int>> WhishListCount( string userId)
        {
            try
            {
                var entity = _context.Whislists.Where(x=>x.UserID == userId).ToList();

                if (entity != null)
                {

                    return _response.Create(false, "All Data Fetched.",entity.Count());

                }

                return _response.Create(false, "Record does not exist.",0);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, 0);
            }
        }
    }
}
