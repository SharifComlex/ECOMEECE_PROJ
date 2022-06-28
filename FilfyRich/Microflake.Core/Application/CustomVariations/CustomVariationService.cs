using Microflake.Core.Domain;
using Microflake.Core.Persistence;
using Microflake.Core.Utilities.Logger;
using Microflake.Core.Utilities.Response;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.CustomVariations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Microflake.Core.Application.CustomVariations
{
    public class CustomVariationService : ICustomVariationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResponse _response;
        private readonly ILogger _logger; 
        public CustomVariationService(ApplicationDbContext context,
            ILogger logger, IResponse response)
        {
            _context = context;
            _response = response;
            _logger = logger;
        }
       
        public async Task<ServiceResponse<List<ListVariation>>> List()
        {
            try
            {
                var list = await _context
                    .CustomVariations
                    .Select(x => new ListVariation
                    {
                        Id = x.Id,
                        ColorName = x.Cap.Name,
                        ItemName = x.Badge.Name,
                        FrontImage = x.FrontImage,
                        BackImage = x.BackImage,
                        BottomImage = x.BottomImage
                    }).ToListAsync();

                return _response.Create(true, "All record has been fetched", list);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new List<ListVariation>());
            }
        }
       

        public async Task<ServiceResponse<ListVariation>> Create(CreateVariation model, string userId)
        {
            try
            {
                
                var entity = new CustomVariation();

                entity.CapId = model.ColorId;
                entity.BadgeId = model.ItemId;

                _context.CustomVariations.Add(entity);

                var entityResult = await _context.SaveChangesAsync();
               
                if (!string.IsNullOrEmpty(model.FrontImage))
                {
                    entity.FrontImage = model.FrontImage;
                    _context.Entry(entity).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                  
                }

                //if (!string.IsNullOrEmpty(model.BackImage))
                //{
                //    entity.BackImage = model.BackImage;
                //    _context.Entry(entity).State = EntityState.Modified;

                //    await _context.SaveChangesAsync();

                //}

                if (!string.IsNullOrEmpty(model.BottomImage))
                {
                    entity.BottomImage = model.BottomImage;
                    _context.Entry(entity).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                }

                return _response.Create(entityResult, 1, await FindSignleListEntity(entity.Id));
            }
            catch (Exception ex)
            {
                return _response.Create(false, ex.Message, new ListVariation());
            }
        }

        public async Task<ServiceResponse<EditVariation>> Edit(long id)
        {
            try
            {
                var entity = await _context.CustomVariations.SingleOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    return _response.Create(false, "Fetched", new EditVariation());
                }

                return _response.Create(true, "Fetched", new EditVariation
                {
                    Id = entity.Id,
                    ColorId = entity.CapId,
                    ItemId = entity.BadgeId,
                    FrontImage = entity.FrontImage,
                    BottomImage = entity.BackImage,
                    //BackImage = entity.BackImage
                });
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new EditVariation());
            }
        }

        public async Task<ServiceResponse<ListVariation>> Update(EditVariation model, string userId)
        {
            try
            {
                var entity = _context.CustomVariations.SingleOrDefault(x => x.Id == model.Id);

                if (entity == null)
                {
                    return _response.Create(false, "id deos not exits in the system", new ListVariation());
                }

                entity.CapId = model.ColorId;
                entity.BadgeId = model.ItemId;

                _context.Entry(entity).State = EntityState.Modified;

                var entityResult = await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(model.FrontImage))
                {
                    entity.FrontImage = model.FrontImage;
                    _context.Entry(entity).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                }

                //if (!string.IsNullOrEmpty(model.BackImage))
                //{
                //    entity.BackImage = model.BackImage;
                //    _context.Entry(entity).State = EntityState.Modified;

                //    await _context.SaveChangesAsync();

                //}

                if (!string.IsNullOrEmpty(model.BottomImage))
                {
                    entity.BottomImage = model.BottomImage;
                    _context.Entry(entity).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                }

                return _response.Create(entityResult, 2, await FindSignleListEntity(entity.Id));
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return _response.Create(false, ex.Message, new ListVariation());
            }
        }

        public async Task<ServiceResponse<long>> Delete(long id)
        {
            try
            {
                var entity = _context.CustomVariations.Find(id);

                if (entity != null)
                {
                    _context.CustomVariations.Remove(entity);

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

        public async Task<ListVariation> FindSignleListEntity(long Id)
        {
            return await _context
                     .CustomVariations
                     .Select(x => new ListVariation
                     {
                         Id = x.Id,
                         ColorName = x.Cap.Name,
                         ItemName = x.Badge.Name,
                         FrontImage = x.FrontImage,
                         BackImage = x.BackImage,
                         BottomImage = x.BottomImage
                     }).SingleOrDefaultAsync(x => x.Id == Id);
        }

    }
}
