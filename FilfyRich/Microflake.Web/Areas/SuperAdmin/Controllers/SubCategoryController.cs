using Microflake.Core.Application.Categories;
using Microflake.Core.Application.SubCategories;
using Microflake.Core.Persistence;
using Microflake.Core.ViewModel.SubCategories;
using Microflake.Web.Controllers;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microflake.Web.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SubCategoryController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _CategoriesService;

        private readonly ISubCategoryService _entityService;

        public SubCategoryController(ApplicationDbContext context,ICategoryService CategoriesService, ISubCategoryService entityService)
        {    _context =context;
            _CategoriesService = CategoriesService;
            _entityService = entityService;
        }

        public async Task<ActionResult> Index()
        {
            var list = await _entityService.List();
            return View(list.Data);
        }
        public async Task<ActionResult> GetList()
        {
            var result = await _entityService.List();

            return Json(new
            {
                status = result.Success,
                message = result.Message,
                data = result.Data
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: Categories/Create
        public async Task<ActionResult> Create()
        {
            var categoryResult = await _CategoriesService.ToList();

            ViewBag.CategoryId = new MultiSelectList(categoryResult.Data.ToList(), "Id", "Name");

            return PartialView();
        }
        // POST: Categories/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateSubCategory model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationErrors();
            }

            var result = await _entityService.Create(model, User.Identity.GetUserId());

            return Json(new
            {
                status = result.Success,
                message = result.Message,
                data = result.Data
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var result = await _entityService.Edit(id);

            var categoryResult = await _CategoriesService.ToList();

            ViewBag.CategoryId = new MultiSelectList(categoryResult.Data.ToList(), "Id", "Name");

            return PartialView(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditSubCategory model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationErrors();
            }

            var result = await _entityService.Update(model, User.Identity.GetUserId());

            return Json(new
            {
                status = result.Success,
                message = result.Message,
                data = result.Data
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Remove(long Id)
        {
            var result = await _entityService.Delete(Id);

            return Json(new
            {
                status = result.Success,
                message = result.Message,
                data = result.Data
            }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ByCategoriesId(long? Id)
        {
            if (Id.HasValue)
            {
                var options = "<option value=\"\">" + "Select Sub Categories" + "</option>";
                var result = await _entityService.List();
                
                var SubCategoriess = result.Data.Where(x => x.CategoryId == Id).ToList();

                foreach (var item in SubCategoriess)
                {

                    options += "<option value=\"" + item.Id + "\">" + item.Name + "</option>";

                }
                return Content(options);
                //}
            }

            return Content("<option value=\"\">" + "Select Sub Categories" + "</option>");
        }

    }
}