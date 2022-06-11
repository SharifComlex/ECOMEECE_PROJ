using Microflake.Core.Domain;
using Microflake.Core.Persistence;
using Microflake.Core.Utilities.Logger;
using Microflake.Core.Utilities.Response;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.CustomItems;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Microflake.Core.Application.CustomItems
{
    public class CustomItemService : ICustomItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResponse _response;
        private readonly ILogger _logger; 
        public CustomItemService(ApplicationDbContext context,
            ILogger logger, IResponse response)
        {
            _context = context;
            _response = response;
            _logger = logger;
        }
       
        public async Task<ServiceResponse<List<ListProduct>>> List()
        {
            try
            {
                var list = await _context
                    .CustomItems
                    .Select(x => new ListProduct
                    {
                        Id= x.Id,
                        Name = x.Name,
                        CategoryName = x.Category.Name,
                        Status = x.Status,
                        Created = x.CreatedAt,
                        SellPrice = x.SellPrice,
                        Image = x.Image,
                        Discount = x.Discount,

                    }).ToListAsync();

                return _response.Create(true, "All record has been fetched", list);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListProduct>());
            }
        }

        public async Task<ServiceResponse<List<ListProduct>>> List(long Id)
        {
            try
            {
                var list = await _context
                    .CustomItems
                    .Select(x => new ListProduct
                    {
                        Id = x.Id,
                        Name = x.Name,
                       CategoryId = x.CategoryId

                    }).Where(x=> x.CategoryId == Id).ToListAsync();

                return _response.Create(true, "All record has been fetched", list);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListProduct>());
            }
        }

        public async Task<ServiceResponse<ListProduct>> Create(CreateProduct model, string userId)
        {
            try
            {
                
                var entity = new CustomItem();

                entity.CategoryId = model.CategoryId;
                entity.Name = model.Name;
                entity.Status = true;
                entity.BuyPrice = model.BuyPrice;
                entity.SellPrice = model.SellPrice;
                entity.Discount = model.Discount;

                entity.CreatedAt = DateTime.UtcNow;
;

                /*-----------------------------------------------------*/

                _context.CustomItems.Add(entity);
                var entityResult = await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(model.Image))
                {
                    string path = Path.Combine(HttpContext.Current.Server.MapPath(Config.CustomProducts), model.Image);
                    string profilePath = Path.Combine(HttpContext.Current.Server.MapPath(Config.CustomProducts), entity.Id + ".jpg");

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(Config.CustomProducts)))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(Config.CustomProducts));
                    }

                    entity.Image = model.Image;
                    _context.Entry(entity).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                  
                }

                return _response.Create(entityResult, 1, await FindSignleListEntity(entity.Id));
            }
            catch (Exception ex)
            {
                return _response.Create(false, ex.Message, new ListProduct());
            }
        }

        public async Task<ServiceResponse<EditProduct>> Edit(long id)
        {
            try
            {
                var entity = await _context.CustomItems.SingleOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    return _response.Create(false, "Fetched", new EditProduct());
                }

                return _response.Create(true, "Fetched", new EditProduct
                {
                    Id = entity.Id,
                    SellPrice = entity.SellPrice,
                    Status = entity.Status,
                    CategoryId = entity.CategoryId,
                    BuyPrice = entity.BuyPrice,
                    Name = entity.Name,
                    Image = entity.Image,
                    Discount = entity.Discount
                });
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new EditProduct());
            }
        }

        public async Task<ServiceResponse<ListProduct>> Update(EditProduct model, string userId)
        {
            try
            {
                var entity = _context.CustomItems.SingleOrDefault(x => x.Id == model.Id);

                if (entity == null)
                {
                    return _response.Create(false, "id deos not exits in the system", new ListProduct());
                }

                entity.CategoryId = model.CategoryId;
                entity.SellPrice = model.SellPrice;
                entity.Name = model.Name;
                entity.Status = model.Status;
                entity.BuyPrice = model.BuyPrice;
                entity.Discount = model.Discount;

                _context.Entry(entity).State = EntityState.Modified;
                var entityResult = await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(model.Image))
                {
                    string path = Path.Combine(HttpContext.Current.Server.MapPath(Config.CustomProducts), model.Image);
                    string profilePath = Path.Combine(HttpContext.Current.Server.MapPath(Config.CustomProducts), entity.Id + ".jpg");

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(Config.CustomProducts)))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(Config.CustomProducts));
                    }
                    entity.Image = model.Image;
                    _context.Entry(entity).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                   
                }

                return _response.Create(entityResult, 2, await FindSignleListEntity(entity.Id));
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new ListProduct());
            }
        }

        public async Task<ServiceResponse<long>> Delete(long id)
        {
            try
            {
                var entity = _context.CustomItems.Find(id);

                if (entity != null)
                {
                    _context.CustomItems.Remove(entity);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return _response.Create(true, "Record has been removed.", 0L);
                    }
                    else
                    {
                        return _response.Create(false, "Error while updating the record.", entity.Id);
                    }
                }

                return _response.Create(false, "Record does not exist.", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, 0L);
            }
        }

        public async Task<ListProduct> FindSignleListEntity(long Id)
        {
            return await _context
                     .CustomItems
                     .Select(x => new ListProduct
                     {
                         Id = x.Id,
                         Name = x.Name,
                         CategoryName = x.Category.Name,
                         Status = x.Status,
                         Created = x.CreatedAt,
                         SellPrice = x.SellPrice,
                         Image = x.Image,
                         Discount = x.Discount,
                     }).SingleOrDefaultAsync(x => x.Id == Id);
        }

    }
}
