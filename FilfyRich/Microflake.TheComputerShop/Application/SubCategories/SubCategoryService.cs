using Microflake.TheComputerShop.Domain;
using Microflake.TheComputerShop.Persistence;
using Microflake.TheComputerShop.Utilities.Logger;
using Microflake.TheComputerShop.Utilities.Response;
using Microflake.TheComputerShop.ViewModel;
using Microflake.TheComputerShop.ViewModel.Category;
using Microflake.TheComputerShop.ViewModel.SubCategories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microflake.TheComputerShop.Application.SubCategories
{
 public  class SubCategorieservice : ISubCategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResponse _response;

        private readonly ILogger _logger;

        public SubCategorieservice(ApplicationDbContext context,
            ILogger logger, IResponse response)
        {
            _context = context;
            _response = response;
        }
        public async Task<ServiceResponse<List<ListSubCategory>>> List()
        {
            try
            {
                var list = await _context
                    .SubCategories
                    .AsNoTracking()
                    .OrderBy(x => x.English)
                    .ToListAsync();

                return _response.Create(true, "Fatched", list.Select(x => new ListSubCategory
                {
                    Id = x.Id,
                    English = x.English,
                    Arabic = x.Arabic,
                    Status = x.Status
                }).ToList());
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListSubCategory>());

            }
        }

        public async Task<ServiceResponse<SelectList>> SelectList(long? Id)
        {
            try
            {
                var list = await _context
                    .SubCategories

                    .Select(x => new ItemSelectList
                    {
                        Id = x.Id,
                        Title = x.English,
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

        public async Task<ServiceResponse<SubCategory>> Get(long id)
        {
            try
            {
                var entity = await _context.SubCategories.SingleOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    return _response.Create(false, "Fetched", new SubCategory());
                }

                return _response.Create(true, "Fetched", entity);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new SubCategory());
            }
        }

        public async Task<ServiceResponse<EditSubCategory>> Edit(long id)
        {
            try
            {
                var entity = await _context.SubCategories.SingleOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    return _response.Create(false, "Fetched", new EditSubCategory());
                }

                return _response.Create(true, "Fetched", new EditSubCategory
                {
                    Id = entity.Id,
                    Status = entity.Status,
                    CategoryId = entity.CategoryId,
                    English = entity.English,
                    Arabic = entity.Arabic,

                });
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new EditSubCategory());
            }
        }

        public async Task<ServiceResponse<ListSubCategory>> Create(CreateSubCategory model, string userId)
        {
            try
            {
                var entity = new SubCategory();

                entity.CategoryId = model.CategoryId;
                entity.English = model.English;
                entity.Arabic = model.Arabic;
                entity.Status = true;

                /*-------------- CommonEntities Entries ---------------*/

                entity.CreatedAt = DateTime.UtcNow;
                entity.ModifiedAt = DateTime.UtcNow;
                entity.CreatedById = userId;
                entity.ModifiedById = userId;

                /*-----------------------------------------------------*/

                _context.SubCategories.Add(entity);
                var entityResult = await _context.SaveChangesAsync();

                return _response.Create(entityResult, 1, await FindSignleListEntity(entity.Id));
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new ListSubCategory());
            }
        }

        public async Task<ServiceResponse<ListSubCategory>> Update(EditSubCategory model, string userId)
        {
            try
            {
                var entity = _context.SubCategories.SingleOrDefault(x => x.Id == model.Id);

                if (entity == null)
                {
                    return _response.Create(false, "id deos not exits in the system", new ListSubCategory());
                }

                entity.CategoryId = model.CategoryId;

                entity.English = model.English;
                entity.Arabic = model.Arabic;
                entity.Status = model.Status;

                entity.ModifiedAt = DateTime.UtcNow;
                entity.ModifiedById = userId;

                _context.Entry(entity).State = EntityState.Modified;
                var entityResult = await _context.SaveChangesAsync();


                return _response.Create(entityResult, 2, await FindSignleListEntity(entity.Id));
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new ListSubCategory());
            }
        }


        public async Task<ServiceResponse<long>> Delete(long id)
        {
            try
            {

                var entity = _context.SubCategories.Find(id);
                var result = 0;
                if (entity == null)
                {
                    return _response.Create(false, "record does not exists.", 0L);
                }
                else if (entity != null)
                {
                    var used = _context.Products.Where(x => x.SubCategoryId == entity.Id).ToList();
                    if (used.Count() > 0)
                    {
                        result = 0;
                    }
                    else
                    {
                        _context.SubCategories.Remove(entity);
                        result = await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    return _response.Create(false, "Record does not exist.", entity.Id);
                }

                if (result == 0)
                {
                    return _response.Create(false, "It is used by other records", id);
                }
                else if (result == 1)
                {
                    return _response.Create(true, "Record has been removed.", id);
                }
                else
                {
                    return _response.Create(false, "Error while updating the record.", id);
                }

                //return _response.CreateResponse(true, "record has been removed.", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, 0L);
            }
        }

        //public async Task<ServiceResponse<long>> Delete(long id)
        //{
        //    try
        //    {
        //        var entity = _context.SubCategories.Find(id);

        //        if (entity != null)
        //        {
        //            _context.SubCategories.Remove(entity);

        //            if (await _context.SaveChangesAsync() > 0)
        //            {
        //                return _response.Create(true, "Record has been removed.", 0L);
        //            }
        //            else
        //            {
        //                return _response.Create(false, "Error while updating the record.", entity.Id);
        //            }
        //        }

        //        return _response.Create(false, "Record does not exist.", entity.Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Log(ex);
        //        return _response.Create(false, ex.Message, 0L);
        //    }
        //}

        public async Task<ListSubCategory> FindSignleListEntity(long Id)
        {
            return await _context
                     .SubCategories
                     .Select(x => new ListSubCategory
                     {
                         Id = x.Id,
                         CategoryId = x.CategoryId,
                         CategoryName = x.Category.English,
                        English = x.English,
                        Arabic = x.Arabic, 
                        Status = x.Status,


                         Created = x.CreatedAt
                     }).SingleOrDefaultAsync(x => x.Id == Id);
        }
    }
}
