using ECap.Admin.Areas.SuperAdmin.Models.Products;
using ECap.Core.Database;
using ECap.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECap.Web.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Area("SuperAdmin")]
    public class ProductsController : Controller
    {
        private ECapDbContext _dbContext;

        public ProductsController(ECapDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _dbContext.Products.Select(x => new ListProduct
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Category = x.SubCategory.Category.Name + " -> " + x.SubCategory.Name,
                Image = x.Image,
                Status = x.Status
            })
            .ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.CategoryId = new SelectList(await _dbContext.Categories.ToListAsync(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProduct model)
        {
            if (ModelState.IsValid)
            {
                if (!_dbContext.Products.Any(u => u.Name == model.Name))
                {
                    var entity = new Product
                    {
                        Name = model.Name,
                        Price = model.Price,
                        Description = model.Description,
                        SubCategoryId = model.CategoryId,
                        CreatedAt = DateTime.UtcNow,
                        Status = true,
                    };

                    entity.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                   _dbContext.Products.Add(entity);
                    if (await _dbContext.SaveChangesAsync() > 0) {

                        if (model.File != null) {
                            entity.Image = entity.Id + ".jpg";
                            _dbContext.Entry(entity).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            try
                            {
                                var rootPath = Path.GetFullPath(Directory.GetCurrentDirectory() + "/wwwroot/products");

                                if (!Directory.Exists(rootPath))
                                {
                                    Directory.CreateDirectory(rootPath);
                                }

                                var filePath = rootPath + "/" + entity.Image;

                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }

                                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    model.File.CopyTo(fileStream);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        return RedirectToAction("Index", new { Area = "SuperAdmin"});
                    }
                }
                else
                {
                    ModelState.AddModelError("Name", "This product name is not available");
                }
            }

            ViewBag.CategoryId = new SelectList(await _dbContext.Categories.ToListAsync(), "Id", "Name");

            return View(model);
        }

        public async Task<IActionResult> Edit(long Id)
        {
            var model = await _dbContext.Products
            .Select(x => new EditProduct
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.SubCategory.CategoryId,
                SubCategoryId = x.SubCategoryId,
                Price = x.Price,
                Description = x.Description
            })
            .FirstOrDefaultAsync(x => x.Id == Id);

            ViewBag.CategoryId = new SelectList(await _dbContext.Categories.ToListAsync(), "Id", "Name", model.CategoryId);
            ViewBag.SubCategoryId = new SelectList(await _dbContext.SubCategories.ToListAsync(), "Id", "Name", model.SubCategoryId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProduct model)
        {
            if (ModelState.IsValid)
            {

                var entity = await _dbContext.Products
                            .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (entity != null)
                {
                    if (!_dbContext.SubCategories.Any(u => u.Name == model.Name && u.Id != model.Id))
                    {
                        if (model.File != null)
                        {
                            entity.Image = entity.Id + ".jpg?v=" + DateTime.UtcNow.Ticks;
                            _dbContext.Entry(entity).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            try
                            {
                                var rootPath = Path.GetFullPath(Directory.GetCurrentDirectory() + "/wwwroot/products");

                                if (!Directory.Exists(rootPath))
                                {
                                    Directory.CreateDirectory(rootPath);
                                }

                                var filePath = rootPath + "/" + entity.Id + ".jpg";

                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }

                                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    model.File.CopyTo(fileStream);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }

                        entity.Name = model.Name;
                        entity.Price = model.Price;
                        entity.Description = model.Description;
                        entity.SubCategoryId = model.SubCategoryId;

                        _dbContext.Entry(entity).State = EntityState.Modified;
                        if (await _dbContext.SaveChangesAsync() > 0)
                        {
                            return RedirectToAction("Index", new { Area = "SuperAdmin" });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Name", "This product name is not available");
                    }
                }
            }

            ViewBag.CategoryId = new SelectList(await _dbContext.Categories.ToListAsync(), "Id", "Name", model.CategoryId);
            ViewBag.SubCategoryId = new SelectList(await _dbContext.SubCategories.ToListAsync(), "Id", "Name", model.SubCategoryId);

            return View(model);
        }


        public async Task<IActionResult> UpdateStatus(long Id)
        {
            var entity = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == Id);

            if (entity != null)
            {
                if (entity.Status)
                {
                    entity.Status = false;
                }
                else
                {
                    entity.Status = true;
                }

                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { Area = "SuperAdmin" });
        }
    }
}
