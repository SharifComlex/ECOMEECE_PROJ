using ECap.Admin.Areas.SuperAdmin.Models.SubCategories;
using ECap.Core.Database;
using ECap.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECap.Web.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Area("SuperAdmin")]
    public class SubCategoriesController : Controller
    {
        private ECapDbContext _dbContext;

        public SubCategoriesController(ECapDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _dbContext.SubCategories.Select(x => new ListSubCategory
            {
                Id = x.Id,
                Name = x.Name,
                CategoryName = x.Category.Name,
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
        public async Task<IActionResult> Create(CreateSubCategory model)
        {
            if (ModelState.IsValid)
            {
                if (!_dbContext.SubCategories.Any(u => u.Name == model.Name))
                {
                    var entity = new SubCategory
                    {
                        Name = model.Name,
                        CategoryId = model.CategoryId,
                        CreatedAt = DateTime.UtcNow,
                        Status = true,
                    };

                    entity.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                   _dbContext.SubCategories.Add(entity);
                    if (await _dbContext.SaveChangesAsync() > 0) {
                        return RedirectToAction("Index", new { Area = "SuperAdmin"});
                    }
                }
                else
                {
                    ModelState.AddModelError("Name", "This subCategory name is not available");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(long Id)
        {
            var model = await _dbContext.SubCategories
            .Select(x => new EditSubCategory
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryId
            })
            .FirstOrDefaultAsync(x => x.Id == Id);

            ViewBag.CategoryId = new SelectList(await _dbContext.Categories.ToListAsync(), "Id", "Name", model.CategoryId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSubCategory model)
        {
            if (ModelState.IsValid)
            {

                var entity = await _dbContext.SubCategories
                            .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (entity != null)
                {
                    if (!_dbContext.SubCategories.Any(u => u.Name == model.Name && u.Id != model.Id))
                    {
                        entity.Name = model.Name;

                        _dbContext.Entry(entity).State = EntityState.Modified;
                        if (await _dbContext.SaveChangesAsync() > 0)
                        {
                            return RedirectToAction("Index", new { Area = "SuperAdmin" });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Name", "This subCategory name is not available");
                    }
                }
            }

            ViewBag.CategoryId = new SelectList(await _dbContext.Categories.ToListAsync(), "Id", "Name", model.CategoryId);

            return View(model);
        }


        public async Task<IActionResult> UpdateStatus(long Id)
        {
            var entity = await _dbContext.SubCategories.FirstOrDefaultAsync(x => x.Id == Id);

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

        public async Task<IActionResult> GetCategoryId(long Id)
        {
            var data = await _dbContext.SubCategories.Select(x => new ListSubCategory
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                Status = x.Status
            })
            .Where(x=> x.CategoryId == Id)
            .ToListAsync();

            return PartialView(data);
        }
    }
}
