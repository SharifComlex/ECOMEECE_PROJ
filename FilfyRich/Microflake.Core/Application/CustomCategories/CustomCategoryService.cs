using Microflake.Core.Domain;
using Microflake.Core.Persistence;
using Microflake.Core.Utilities.Logger;
using Microflake.Core.Utilities.Response;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.CustomCategories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microflake.Core.Application.CustomCategories
{
    public class CustomCategoryService : ICustomCategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IResponse _response;

        public CustomCategoryService(ApplicationDbContext context, ILogger logger, IResponse response)
        {
            _context = context;
            _logger = logger;
            _response = response;
        }

        public async Task<ServiceResponse<List<ListCategory>>> ToList()
        {
            try
            {
                var list = await _context
                    .CustomCategories
                    .AsNoTracking()
                    .OrderBy(x => x.Name)
                    .ToListAsync();

                return _response.Create(true, "Fatched", list.Select(x => new ListCategory
                {
                    Id = x.Id,
                    Name = x.Name,
                    Status = x.Status
                }).ToList());
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListCategory>());

            }
        }
        public async Task<ServiceResponse<SelectList>> SelectList(long? Id)
        {
            try
            {
                var list = await _context
                    .CustomCategories

                    .Select(x => new ItemSelectList
                    {
                        Id = x.Id,
                        //Title = x.Name,
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

        public async Task<ServiceResponse<ListCategory>> Create(CreateCategory model, string userId)
        {
            try
            {
                var entity = new CustomCategory();

                entity.Name = model.Name;

                _context.CustomCategories.Add(entity);
                var result = await _context.SaveChangesAsync();

                if (result == 1)
                {
                    return _response.Create(true, "Added", await Find(entity.Id));
                }
                else
                {
                    return _response.Create(false, "Error while creating a new record.", new ListCategory());
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new ListCategory());
            }

        }

        public async Task<ServiceResponse<EditCategory>> Edit(long id)
        {
            try
            {
                var entity = await _context
                    .CustomCategories
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (entity != null)
                {
                    return _response.Create(true, "Fatched", new EditCategory {
                        Id = entity.Id,
                        Name = entity.Name,
                        Status = entity.Status
                    });
                }

                return _response.Create(false, "Not Fatched", new EditCategory());
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new EditCategory());
            }
        }

        public async Task<ServiceResponse<ListCategory>> Update(EditCategory model, string userId)
        {
            try
            {
                var entity = _context.CustomCategories.Find(model.Id);

                entity.Name = model.Name;

                _context.Entry(entity).State =  EntityState.Modified;

                var result = await _context.SaveChangesAsync();
                
                if (result > 0)
                {
                    return _response.Create(true, "Record has been updated", await Find(entity.Id));
                }
                else
                {
                    return _response.Create(false, "Error while updating the record.", new ListCategory());
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new ListCategory());
            }
        }


        public async Task<ServiceResponse<long>> Remove(long id)
        {
            try
            {

                var entity = _context.CustomCategories.Find(id);
                var result = 0;
                if (entity == null)
                {
                    return _response.Create(false, "record does not exists.", 0L);
                }
                else if (entity != null)
                {
                    var used = _context.SubCategories.Where(x => x.CategoryId == entity.Id).ToList();
                    if (used.Count() > 0)
                    {
                        result = 0;
                    }
                    else
                    {
                        _context.CustomCategories.Remove(entity);
                        result = await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    return _response.Create(false, "Record does not exist.", entity.Id);
                }

                if (result == 0)
                {
                    return _response.Create(false,"It is used by other records", id);
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

      
        private async Task<ListCategory> Find(long Id)
        {
            var model = await _context
                    .CustomCategories
                    .Select(x => new ListCategory
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Status = x.Status
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x=>x.Id == Id);
            return model;
        }
    }
}