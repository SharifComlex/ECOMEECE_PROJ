using Microflake.Core.Domain;
using Microflake.Core.Persistence;
using Microflake.Core.Utilities.Logger;
using Microflake.Core.Utilities.Response;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.Products;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.Core.Application.Products
{
    public class ProductService :IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResponse _response;
        private readonly string _cartId;

        private readonly ILogger _logger;

        public ProductService(ApplicationDbContext context,
            ILogger logger, IResponse response)
        {
            _context = context;
            _response = response;
          
        }
       
        public async Task<ServiceResponse<List<ListProduct>>> List()
        {
            try
            {
                var list = await _context
                    .Products
                    .Where(x=>x.DealOfTheWeek == false)
                    .Select(x => new ListProduct
                    {
                       Id= x.Id,
                        Name = x.Name,
                        CategoryName = x.SubCategory.Category.Name,
                        Status = x.Status,
                        Created = x.CreatedAt,
                        IsFeatured = x.IsFeatured,
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
                return _response.Create(false, ex.Message, new List<ListProduct>());
            }
        }
       
        public async Task<ServiceResponse<List<ListProduct>>> ProductList(int Page, int recordSize, int? categoryID)
        {
            try
            {
                var skip = (Page - 1) * recordSize;
                var list = await _context
                    .Products
                    .Select(x => new ListProduct
                    {
                        Id = x.Id,
                        //Name = x.Title,
                        Name = x.Name,
                        Status = x.Status,
                        Created = x.CreatedAt,
                        IsFeatured = x.IsFeatured,
                        SellPrice = x.SellPrice

                    })
                    .OrderBy(x=>x.Id).Skip(skip).Take(recordSize)
                    .ToListAsync();

                return _response.Create(true, "All record has been fetched", list);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListProduct>());
            }
        }
        public IEnumerable<Product> List(int? subcategoryID, string searchTerm, int Page, int recordSize, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy)
        {
            var currency = _context.Currencies.ToList();
            var products = _context.Products.ToList();
            if (categoryID.HasValue)
            {
                products = products.Where(x => x.SubCategory.Category.Id == categoryID.Value).ToList();
            }
            if (subcategoryID.HasValue)
            {
                products = products.Where(x => x.SubCategory.Id == subcategoryID.Value).ToList();
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            if (minimumPrice.HasValue)
            {
                products = products.Where(x => x.SellPrice >= minimumPrice.Value).ToList();
            }

            if (maximumPrice.HasValue)
            {
                products = products.Where(x => x.SellPrice <= maximumPrice.Value).ToList();
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {
                    case 2:
                        products = products.OrderByDescending(x => x.Id).ToList();
                        break;
                    case 3:
                        products = products.OrderBy(x => x.SellPrice).ToList();
                        break;
                    case 5:
                        products = products.Where(x => x.SellPrice >= 0 && x.SellPrice<= 500).ToList();
                        break;
                    case 6:
                        products = products.Where(x => x.SellPrice >= 500 && x.SellPrice <= 1000).ToList();
                        break;
                    case 7:
                        products = products.Where(x => x.SellPrice >= 1000 && x.SellPrice <= 1500).ToList();
                        break;
                    case 8:
                        products = products.Where(x => x.SellPrice >= 1500 && x.SellPrice <= 2000).ToList();
                        break;
                    default:
                        products = products.OrderByDescending(x => x.SellPrice).ToList();
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
           //var basecurrency =  currency.FirstOrDefault(x => x.Base_Currency == true);
           // var cookiecurrency = currency.FirstOrDefault(x => x.Name == Currency);
           // if (cookiecurrency != null )
           // {
           //     foreach (var item in products)
           //     {
           //       var i =   item.SellPrice* cookiecurrency.Currency_Rate;
           //         //if (basecurrency.Currency_Rate < cookiecurrency.Currency_Rate)
           //         //{
           //         if ( i >item.SellPrice )
           //         {
           //             item.SellPrice = i;
           //         }
                   
           //         //}
           //         //else
           //         //{

           //         //}

           //     }

           // }
         
            return products.Skip((Page - 1) * recordSize).Take(recordSize).ToList();


        }
        public async Task<IEnumerable<CartItem>> GetCartItemsAsync()
        {
            return await _context.CartItems.Include("Product")
                .Where(c => c.CartId == _cartId).ToArrayAsync();
        }
        public async Task AddAsync(int productId)
        {
            var product = await _context.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                // TODO: throw exception or do something
                return;
            }

            var cartItem = await _context.CartItems
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

                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        public double GetMaximumPrice()
        {
            if (_context.Products.Count()>0)
            {
                var maxPrice = _context.Products.Max(x => x.SellPrice);
                return maxPrice;
            }
            return 0;


            // skip takes ordered values kis bhi bse pay karlo
           
            
                
           
        }
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

        public async Task<ServiceResponse<EditProduct>> Edit(long id)
        {
            try
            {
                var entity = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    return _response.Create(false, "Fetched", new EditProduct());
                }

                return _response.Create(true, "Fetched", new EditProduct
                {
                   Id=entity.Id,
                   SellPrice =entity.SellPrice,
                    Status = entity.Status,
                    SubCategoryId = entity.SubCategoryId,
                    Price= entity.Price,
                    Description = entity.Description,
                    IsFeatured = entity.IsFeatured,
                    IsNew = entity.IsNew,
                    Name = entity.Name,
                    ProductImage = entity.Image,
                  ProductImage1 = entity.Image1,
                  Qty = entity.Qty ,
                  Discount = entity.Discount

                });
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new EditProduct());
            }
        }

        public async Task<ServiceResponse<ListProduct>> Create(CreateProduct model, string userId)
        {
            try
            {
                
                var entity = new Product();

                entity.SubCategoryId = model.SubCategoryId;
                entity.Name = model.Name;
                entity.Status = true;
                entity.IsFeatured = model.IsFeatured;
                entity.IsNew = model.IsNew;
                entity.Description = model.Description;
                entity.Price = model.Price;
                entity.SellPrice = model.SellPrice;
                entity.Discount = model.Discount;
                entity.Qty = model.Qty;
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

                    //    entity.Image = model.ProductImage;
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

                    //    entity.Image1 = model.ProductImage1;
                    //    _context.Entry(entity).State = EntityState.Modified;

                    //    await _context.SaveChangesAsync();
                    //}
                }


                return _response.Create(entityResult, 1, await FindSignleListEntity(entity.Id));
            }
            catch (Exception ex)
            {
               // _logger.Log(ex);
                return _response.Create(false, ex.Message, new ListProduct());
            }
        }

        public async Task<ServiceResponse<ListProduct>> Update(EditProduct model, string userId)
        {
            try
            {
                var entity = _context.Products.SingleOrDefault(x => x.Id == model.Id);

                if (entity == null)
                {
                    return _response.Create(false, "id deos not exits in the system", new ListProduct());
                }

                entity.SubCategoryId = model.SubCategoryId;
                entity.SellPrice = model.SellPrice;
                entity.Name = model.Name;
                entity.Status = model.Status;
                entity.Price = model.Price;
                entity.Description = model.Description;
                entity.IsNew = model.IsNew;
                entity.IsFeatured = model.IsFeatured;
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

                    //    entity.Image = model.ProductImage;
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

                    //    entity.Image1 = model.ProductImage1;
                    //    _context.Entry(entity).State = EntityState.Modified;

                    //    await _context.SaveChangesAsync();
                    //}
                }

                return _response.Create(entityResult, 2, await FindSignleListEntity(entity.Id));
            }
            catch (Exception ex)
            {
                //_logger.Log(ex);
                return _response.Create(false, ex.Message, new ListProduct());
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

        public async Task<ListProduct> FindSignleListEntity(long Id)
        {
            return await _context
                     .Products
                     .Select(x => new ListProduct
                     {
                         Id = x.Id,
                         Name = x.Name,
                         CategoryName = x.SubCategory.Category.Name,
                         Status = x.Status,
                         Created = x.CreatedAt,
                         IsFeatured = x.IsFeatured,
                         SellPrice = x.SellPrice,
                         Image = x.Image,
                         Image1 = x.Image1,
                         Discount = x.Discount,
                         Qty = x.Qty
                     }).SingleOrDefaultAsync(x => x.Id == Id);
        }

        
        // for SHop COntroller
        public int ProductCount(int? subcategoryID, string searchTerm, int Page, int recordSize, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy)
        {

            var products = _context.Products.Where(x=>x.DealOfTheWeek == false).AsQueryable();
            if (categoryID.HasValue)
            {
                products = products.Where(x => x.SubCategory.Category.Id == categoryID.Value);
            }
            if (subcategoryID.HasValue)
            {
                products = products.Where(x => x.SubCategory.Id == subcategoryID.Value);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (minimumPrice.HasValue)
            {
                products = products.Where(x => x.SellPrice >= minimumPrice.Value);
            }

            if (maximumPrice.HasValue)
            {
                products = products.Where(x => x.SellPrice <= maximumPrice.Value);
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {
                    case 2:
                        products = products.OrderByDescending(x => x.Id);
                        break;
                    case 3:
                        products = products.OrderBy(x => x.SellPrice);
                        break;
                    default:
                        products = products.OrderByDescending(x => x.SellPrice);
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
            return products.Count();


        }



        // for deal of the week page service 
        public int ProductCountOfDeal(int? subcategoryID, string searchTerm, int Page, int recordSize, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy)
        {

            var products = _context.Products.Where(x => x.DealOfTheWeek == true).AsQueryable();
            if (categoryID.HasValue)
            {
                products = products.Where(x => x.SubCategory.Category.Id == categoryID.Value);
            }
            if (subcategoryID.HasValue)
            {
                products = products.Where(x => x.SubCategory.Id == subcategoryID.Value);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (minimumPrice.HasValue)
            {
                products = products.Where(x => x.SellPrice >= minimumPrice.Value);
            }

            if (maximumPrice.HasValue)
            {
                products = products.Where(x => x.SellPrice <= maximumPrice.Value);
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {
                    case 2:
                        products = products.OrderByDescending(x => x.Id);
                        break;
                    case 3:
                        products = products.OrderBy(x => x.SellPrice);
                        break;
                    default:
                        products = products.OrderByDescending(x => x.SellPrice);
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
            return products.Count();


        }


        // for deal of the week page service 
        public int FeaturedProductCount(int? subcategoryID, string searchTerm, int Page, int recordSize, int? categoryID, int? minimumPrice, int? maximumPrice, int? sortBy)
        {

            var products = _context.Products.Where(x => x.IsFeatured == true).AsQueryable();
            if (categoryID.HasValue)
            {
                products = products.Where(x => x.SubCategory.Category.Id == categoryID.Value);
            }
            if (subcategoryID.HasValue)
            {
                products = products.Where(x => x.SubCategory.Id == subcategoryID.Value);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (minimumPrice.HasValue)
            {
                products = products.Where(x => x.SellPrice >= minimumPrice.Value);
            }

            if (maximumPrice.HasValue)
            {
                products = products.Where(x => x.SellPrice <= maximumPrice.Value);
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {
                    case 2:
                        products = products.OrderByDescending(x => x.Id);
                        break;
                    case 3:
                        products = products.OrderBy(x => x.SellPrice);
                        break;
                    default:
                        products = products.OrderByDescending(x => x.SellPrice);
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
            return products.Count();


        }

        
    }
}
