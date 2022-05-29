using Microflake.TheComputerShop.Application.Categories;
using Microflake.TheComputerShop.ViewModel.Category;
using Microflake.TheComputerShop.Web.Admin.Controllers;
using Microflake.TheComputerShop.Web.Admin.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microflake.TheComputerShop.Web.Admin.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _entityService;

        public CategoriesController(ICategoryService CategoryService)
        {
            _entityService = CategoryService;
            
        }

        public async Task<ActionResult> Index()
        {
            var list = await _entityService.ToList();
            return View(list.Data);
        }

        public async Task<ActionResult> GetList()
        {
            var result = await _entityService.ToList();
            
            return Json(new
            {
                status = result.Success,
                message = result.Message,
                data = result.Data
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Categories/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateCategory model)
        {
            if (model.English == null && model.Arabic == null)
            {
                ModelState.AddModelError("English", Language.Language.This_Field_is_Required);

            }
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    status = false,
                    validation = false,
                    message = Language.Language.Provide_All_Required_Inputs,
                    errors = ModelState.Where(ms => ms.Value.Errors.Any())
                                        .Select(x => new {
                                            x.Key,
                                            Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                                        })
                }
                , JsonRequestBehavior.AllowGet);
            }

            var result = await _entityService.Create(model, User.Identity.GetUserId());
            if (result.Success)
            {
                return Json(new
                {
                    status = result.Success,
                    message = result.Message,
                    data = new
                    {
                        result.Data.Id,
                        result.Data.English,
                        result.Data.Arabic,
                        result.Data.Status,
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = result.Success,
                message = result.Message,
                data = result.Data
            });
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var entity = await _entityService.Edit(id);
            var model = new EditCategory
            {
                Id = entity.Data.Id,
                English = entity.Data.English,
                Arabic = entity.Data.Arabic,
                Status = entity.Data.Status,
            };
            return PartialView(model);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(EditCategory model)
        {
            if (model.English == null && model.Arabic == null)
            {
                ModelState.AddModelError("English", Language.Language.This_Field_is_Required);

            }
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    status = false,
                    validation = false,
                    message = Language.Language.Provide_All_Required_Inputs,
                    errors = ModelState.Where(ms => ms.Value.Errors.Any())
                                        .Select(x => new {
                                            x.Key,
                                            Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                                        })
                }
                , JsonRequestBehavior.AllowGet);
            }


            var result = await _entityService.Update(model, User.Identity.GetUserId());
            if (result.Success)
            {
                return Json(new
                {
                    status = result.Success,
                    message = result.Message,
                    data = new
                    {
                        result.Data.Id,
                        result.Data.English,
                        result.Data.Arabic,
                        result.Data.Status
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = result.Success,
                message = result.Message,
                data = result.Data
            });
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Remove(int id)
        {
            var result = await _entityService.Remove(id);

            return Json(new
            {
                status = result.Success,
                message = result.Message,
                data = result.Data
            },JsonRequestBehavior.AllowGet);
        }

    }
}
