using ECap.Admin.Areas.SuperAdmin.Models.Categories;
using ECap.Core.Database;
using ECap.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECap.Web.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Area("SuperAdmin")]
    public class CategoriesController : Controller
    {
        private ECapDbContext _dbContext;

        public CategoriesController(ECapDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _dbContext.Categories.Select(x => new ListCategory
            {
                Id = x.Id,
                Name = x.Name,
                Status = x.Status
            })
            .ToListAsync();

            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategory model)
        {
            if (ModelState.IsValid)
            {
                if (!_dbContext.Categories.Any(u => u.Name == model.Name))
                {
                    var entity = new Category
                    {
                        Name = model.Name,
                        CreatedAt = DateTime.UtcNow,
                        Status = true,
                    };

                    entity.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    _dbContext.Categories.Add(entity);
                    if (await _dbContext.SaveChangesAsync() > 0) {
                        return RedirectToAction("Index", new { Area = "SuperAdmin" });
                    }
                }
                else
                {
                    ModelState.AddModelError("Name", "This category name is not available");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(long Id)
        {
            var model = await _dbContext.Categories
            .Select(x => new EditCategory
            {
                Id = x.Id,
                Name = x.Name
            })
            .FirstOrDefaultAsync(x => x.Id == Id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCategory model)
        {
            if (ModelState.IsValid)
            {

                var entity = await _dbContext.Categories
                            .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (entity != null)
                {
                    if (!_dbContext.Categories.Any(u => u.Name == model.Name && u.Id != model.Id))
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
                        ModelState.AddModelError("Name", "This category name is not available");
                    }

                   
                }
            }

            return View(model);
        }

        public async Task<IActionResult> UpdateStatus(long Id)
        {
            var entity = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == Id);

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
