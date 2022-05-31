using Microflake.Core.Domain;
using Microflake.Core.Persistence;
using Microflake.Core.Utilities.Logger;
using Microflake.Core.Utilities.Response;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.DealOfTheWeeks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.Core.Application.DealOfTheWeeks
{
   public class DealOfTheWeekService : IDealOfTheWeekService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResponse _response;
        private readonly string _cartId;

        private readonly ILogger _logger;

        public DealOfTheWeekService(ApplicationDbContext context,
            ILogger logger, IResponse response)
        {
            _context = context;
            _response = response;

        }
        public async Task<ServiceResponse<List<ListDealOfTheWeek>>> List()
        {
            try
            {
                var list = await _context
                    .Products
                    .Where(x=>x.DealOfTheWeek==true)
                    .Select(x => new ListDealOfTheWeek
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CategoryName = x.SubCategory.Category.Name,
                        Status = x.Status,
                        Created = x.CreatedAt,
                      
                        SellPrice = x.SellPrice,
                        Image = x.Image,
                        Image1 = x.Image1,
                        Discount = x.Discount,
                        Qty = x.Qty

                    }).ToListAsync();

                return _response.Create(true, "All record has been fetched", list);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListDealOfTheWeek>());
            }
        }

        public async Task<ServiceResponse<List<ListDealOfTheWeek>>> DealOfTheWeekList(int Page, int recordSize, int? categoryID)
        {
            try
            {
                var skip = (Page - 1) * recordSize;
                var list = await _context
                    .Products
                    .Select(x => new ListDealOfTheWeek
                    {
                        Id = x.Id,
                        Name = x.Name,
                        //CategoryName = x.Category.Name,
                        Status = x.Status,
                        Created = x.CreatedAt,
                
                        SellPrice = x.SellPrice

                    })
                    .OrderBy(x => x.Id).Skip(skip).Take(recordSize)
                    .ToListAsync();

                return _response.Create(true, "All record has been fetched", list);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListDealOfTheWeek>());
            }
        }
        public IEnumerable<Product> List(string searchTerm, int Page, int recordSize, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy)
        {

            var DealOfTheWeeks = _context.Products.ToList();
            if (categoryID.HasValue)
            {
                DealOfTheWeeks = DealOfTheWeeks.Where(x => x.SubCategory.Category.Id == categoryID.Value).ToList();
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                DealOfTheWeeks = DealOfTheWeeks.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            if (minimumPrice.HasValue)
            {
                DealOfTheWeeks = DealOfTheWeeks.Where(x => x.Price >= minimumPrice.Value).ToList();
            }

            if (maximumPrice.HasValue)
            {
                DealOfTheWeeks = DealOfTheWeeks.Where(x => x.Price <= maximumPrice.Value).ToList();
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {
                    case 2:
                        DealOfTheWeeks = DealOfTheWeeks.OrderByDescending(x => x.Id).ToList();
                        break;
                    case 3:
                        DealOfTheWeeks = DealOfTheWeeks.OrderBy(x => x.Price).ToList();
                        break;
                    case 5:
                        DealOfTheWeeks = DealOfTheWeeks.Where(x => x.Price >= 0 && x.Price <= 50).ToList();
                        break;
                    case 6:
                        DealOfTheWeeks = DealOfTheWeeks.Where(x => x.Price >= 50 && x.Price <= 100).ToList();
                        break;
                    case 7:
                        DealOfTheWeeks = DealOfTheWeeks.Where(x => x.Price >= 100 && x.Price <= 150).ToList();
                        break;
                    case 8:
                        DealOfTheWeeks = DealOfTheWeeks.Where(x => x.Price >= 150 && x.Price <= 200).ToList();
                        break;
                    default:
                        DealOfTheWeeks = DealOfTheWeeks.OrderByDescending(x => x.Price).ToList();
                        break;
                }
            }
            //if (!string.IsNullOrEmpty(searchTerm))
            //{
            //    jobpost = jobpost.Where(x => x.Title.ToLower().Contains(searchTerm.ToLower()));
            //}
            //if (!string.IsNullOrEmpty(JobType))
            //{
            //    jobpost = jobpost.Where(x => x.JobType.Contains(JobType));
            //}
            //var skip = (Page - 1) * recordSize;
            // skip takes ordered values kis bhi bse pay karlo

            return DealOfTheWeeks.Skip((Page - 1) * recordSize).Take(recordSize).ToList();


        }
        public async Task<IEnumerable<CartItem>> GetCartItemsAsync()
        {
            return await _context.CartItems.Include(x=>x.Product)
                .Where(c => c.CartId == _cartId).ToArrayAsync();
        }
        //public async Task AddAsync(int DealOfTheWeekId)
        //{
        //    var DealOfTheWeek = await _context.DealOfTheWeeks
        //        .SingleOrDefaultAsync(p => p.Id == DealOfTheWeekId);

        //    if (DealOfTheWeek == null)
        //    {
        //        // TODO: throw exception or do something
        //        return;
        //    }

        //    var cartItem = await _context.CartItems
        //        .SingleOrDefaultAsync(c => c.DealOfTheWeekId == DealOfTheWeekId && c.CartId == _cartId);

        //    if (cartItem != null)
        //    {
        //        cartItem.Count++;
        //    }
        //    else
        //    {
        //        cartItem = new CartItem
        //        {
        //            DealOfTheWeekId = DealOfTheWeekId,
        //            CartId = _cartId,
        //            Count = 1,
        //            DateCreated = DateTime.Now
        //        };

        //        _context.CartItems.Add(cartItem);
        //    }

        //    await _context.SaveChangesAsync();
        //}

        //public double GetMaximumPrice()
        //{
        //    if (_context.DealOfTheWeeks.Count() > 0)
        //    {
        //        var maxPrice = _context.DealOfTheWeeks.Max(x => x.Price);
        //        return maxPrice;
        //    }
        //    return 0;


        //    // skip takes ordered values kis bhi bse pay karlo




        //}
        public async Task<ServiceResponse<SelectList>> SelectList(long? Id)
        {
            try
            {
                var list = await _context
                    .Products

                    .Select(x => new ItemSelectList
                    {
                        Id = x.Id,
                        Title = x.Name,
                    }).ToListAsync();

                if (Id.HasValue)
                {
                    var selectList = new SelectList(list, "ID", "Title", Id.HasValue);
                    return _response.Create(true, "All record has been fetched", selectList);
                }
                else
                {
                    var selectList = new SelectList(list, "ID", "Title");
                    return _response.Create(true, "All record has been fetched", selectList);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new SelectList(new List<string>()));
            }
        }

        public async Task<ServiceResponse<Product>> Get(long id)
        {
            try
            {
                var entity = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    return _response.Create(false, "Fetched", new Product());
                }

                return _response.Create(true, "Fetched", entity);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new Product());
            }
        }

        public async Task<ServiceResponse<EditDealOfTheWeek>> Edit(long id)
        {
            try
            {
                var entity = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    return _response.Create(false, "Fetched", new EditDealOfTheWeek());
                }

                return _response.Create(true, "Fetched", new EditDealOfTheWeek
                {
                    Id = entity.Id,
                    SellPrice = entity.SellPrice,
                    Status = entity.Status,
                    CategoryId = entity.SubCategoryId,
                    Price = entity.Price,
                    Description = entity.Description,
                   
                    IsNew = entity.IsNew,
                    ProductImage = entity.Image,
                    ProductImage1 = entity.Image1,
                    Qty = entity.Qty,
                    Discount = entity.Discount

                });
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new EditDealOfTheWeek());
            }
        }

        public async Task<ServiceResponse<ListDealOfTheWeek>> Create(CreateDealOfTheWeek model, string userId)
        {
            try
            {


                var entity = new Product();

                entity.SubCategoryId = model.CategoryId;
                entity.Name = model.Name;
                entity.Status = true;
             
                entity.IsNew = model.IsNew;
                entity.Description = model.Description;
                entity.Price = model.Price;
                entity.SellPrice = model.SellPrice;
                entity.Discount = model.Discount;
                entity.Qty = model.Qty;
                /*-------------- DealOfTheWeek  Entries ---------------*/
                entity.DealOfTheWeek = true;
                entity.DealTillDate = DateTime.Now.AddDays(model.DealForDays);
                /*-------------- CommonEntities Entries ---------------*/

                entity.CreatedAt = DateTime.UtcNow;
                entity.ModifiedAt = DateTime.UtcNow;
                entity.CreatedById = userId;
                entity.ModifiedById = userId;

                /*-----------------------------------------------------*/

                _context.Products.Add(entity);
                var entityResult = await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(model.ProductImage))
                {
                    string path = Path.Combine(HttpContext.Current.Server.MapPath(Config.Products), model.ProductImage);
                    string profilePath = Path.Combine(HttpContext.Current.Server.MapPath(Config.Products), entity.Id + ".jpg");

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(Config.Products)))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(Config.Products));
                    }
                    entity.Image = model.ProductImage;
                    _context.Entry(entity).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    //if (System.IO.File.Exists(path))
                    //{
                    //    if (System.IO.File.Exists(profilePath))
                    //    {
                    //        System.IO.File.Delete(profilePath);
                    //    }

                    //    System.IO.File.Move(path, profilePath);

                    //    entity.Image = model.DealOfTheWeekImage;
                    //    _context.Entry(entity).State = EntityState.Modified;

                    //    await _context.SaveChangesAsync();
                    //}
                }

                if (!string.IsNullOrEmpty(model.ProductImage1))
                {
                    string path = Path.Combine(HttpContext.Current.Server.MapPath(Config.Products), model.ProductImage1);
                    string profilePath = Path.Combine(HttpContext.Current.Server.MapPath(Config.Products), entity.Id + ".jpg");

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(Config.Products)))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(Config.Products));
                    }
                    entity.Image1 = model.ProductImage1;
                    _context.Entry(entity).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    //if (System.IO.File.Exists(path))
                    //{
                    //    if (System.IO.File.Exists(profilePath))
                    //    {
                    //        System.IO.File.Delete(profilePath);
                    //    }

                    //    System.IO.File.Move(path, profilePath);

                    //    entity.Image1 = model.DealOfTheWeekImage1;
                    //    _context.Entry(entity).State = EntityState.Modified;

                    //    await _context.SaveChangesAsync();
                    //}
                }


                return _response.Create(entityResult, 1, await FindSignleListEntity(entity.Id));
            }
            catch (Exception ex)
            {
                // _logger.Log(ex);
                return _response.Create(false, ex.Message, new ListDealOfTheWeek());
            }
        }

        public async Task<ServiceResponse<ListDealOfTheWeek>> Update(EditDealOfTheWeek model, string userId)
        {
            try
            {
                var entity = _context.Products.SingleOrDefault(x => x.Id == model.Id);

                if (entity == null)
                {
                    return _response.Create(false, "id deos not exits in the system", new ListDealOfTheWeek());
                }

                entity.SubCategoryId = model.CategoryId;
                entity.SellPrice = model.SellPrice;
                entity.Name = model.Name;
                entity.Status = model.Status;
                entity.Price = model.Price;
                entity.Description = model.Description;
                entity.IsNew = model.IsNew;
               
                entity.Qty = model.Qty;
                entity.Discount = model.Discount;
                entity.ModifiedAt = DateTime.UtcNow;
                entity.ModifiedById = userId;

                _context.Entry(entity).State = EntityState.Modified;
                var entityResult = await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(model.ProductImage))
                {
                    string path = Path.Combine(HttpContext.Current.Server.MapPath(Config.Products), model.ProductImage);
                    string profilePath = Path.Combine(HttpContext.Current.Server.MapPath(Config.Products), entity.Id + ".jpg");

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(Config.Products)))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(Config.Products));
                    }
                    entity.Image = model.ProductImage;
                    _context.Entry(entity).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    //if (System.IO.File.Exists(path))
                    //{
                    //    if (System.IO.File.Exists(profilePath))
                    //    {
                    //        System.IO.File.Delete(profilePath);
                    //    }

                    //    System.IO.File.Move(path, profilePath);

                    //    entity.Image = model.DealOfTheWeekImage;
                    //    _context.Entry(entity).State = EntityState.Modified;

                    //    await _context.SaveChangesAsync();
                    //}
                }

                if (!string.IsNullOrEmpty(model.ProductImage1))
                {
                    string path = Path.Combine(HttpContext.Current.Server.MapPath(Config.Products), model.ProductImage1);
                    string profilePath = Path.Combine(HttpContext.Current.Server.MapPath(Config.Products), entity.Id + ".jpg");

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(Config.Products)))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(Config.Products));
                    }
                    entity.Image1 = model.ProductImage1;
                    _context.Entry(entity).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    //if (System.IO.File.Exists(path))
                    //{
                    //    if (System.IO.File.Exists(profilePath))
                    //    {
                    //        System.IO.File.Delete(profilePath);
                    //    }

                    //    System.IO.File.Move(path, profilePath);

                    //    entity.Image1 = model.DealOfTheWeekImage1;
                    //    _context.Entry(entity).State = EntityState.Modified;

                    //    await _context.SaveChangesAsync();
                    //}
                }

                return _response.Create(entityResult, 2, await FindSignleListEntity(entity.Id));
            }
            catch (Exception ex)
            {
                //_logger.Log(ex);
                return _response.Create(false, ex.Message, new ListDealOfTheWeek());
            }
        }



        public async Task<ServiceResponse<long>> Delete(long id)
        {
            try
            {
                var entity = _context.Products.Find(id);

                if (entity != null)
                {
                    _context.Products.Remove(entity);

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

        public async Task<ListDealOfTheWeek> FindSignleListEntity(long Id)
        {
            return await _context
                     .Products
                     .Select(x => new ListDealOfTheWeek
                     {
                         Id = x.Id,
                         CategoryId = x.SubCategoryId,
                         //CategoryName = x.Category.Name,
                         Name = x.Name,
                         Status = x.Status,
                         Image = x.Image,
                         Image1 = x.Image1,
                         Qty = x.Qty,
                         Discount = x.Discount,
                         Created = x.CreatedAt
                     }).SingleOrDefaultAsync(x => x.Id == Id);
        }



        public int DealOfTheWeekCount(string searchTerm, int Page, int recordSize, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy)
        {

            var DealOfTheWeeks = _context.Products.AsQueryable();
            if (categoryID.HasValue)
            {
                DealOfTheWeeks = DealOfTheWeeks.Where(x => x.SubCategory.Category.Id == categoryID.Value);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                DealOfTheWeeks = DealOfTheWeeks.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (minimumPrice.HasValue)
            {
                DealOfTheWeeks = DealOfTheWeeks.Where(x => x.Price >= minimumPrice.Value);
            }

            if (maximumPrice.HasValue)
            {
                DealOfTheWeeks = DealOfTheWeeks.Where(x => x.Price <= maximumPrice.Value);
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {
                    case 2:
                        DealOfTheWeeks = DealOfTheWeeks.OrderByDescending(x => x.Id);
                        break;
                    case 3:
                        DealOfTheWeeks = DealOfTheWeeks.OrderBy(x => x.Price);
                        break;
                    default:
                        DealOfTheWeeks = DealOfTheWeeks.OrderByDescending(x => x.Price);
                        break;
                }
            }
            //if (!string.IsNullOrEmpty(searchTerm))
            //{
            //    jobpost = jobpost.Where(x => x.Title.ToLower().Contains(searchTerm.ToLower()));
            //}
            //if (!string.IsNullOrEmpty(JobType))
            //{
            //    jobpost = jobpost.Where(x => x.JobType.Contains(JobType));
            //}

            // skip takes ordered values kis bhi bse pay karlo
            return DealOfTheWeeks.Count();


        }
    }
}
