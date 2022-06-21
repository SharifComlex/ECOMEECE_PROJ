using Microflake.Core;
using Microflake.Core.Application.CustomVariations;
using Microflake.Core.Application.Products;
using Microflake.Core.ViewModel.CustomVariations;
using Microflake.Web.Controllers;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.Web.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class CustomVariationsController : BaseController
    {
        private readonly ICustomVariationService _entityService;
        private readonly IProductService _productService;

        public CustomVariationsController(ICustomVariationService entityService,
           IProductService productService
            )
        {
            _entityService = entityService;
            _productService = productService;
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
            var productResult = await _productService.List();

            ViewBag.ColorId = new SelectList(productResult.Data.Where(x=> x.IsHasVariation).ToList(), "Id", "Name");
            ViewBag.ItemId = new SelectList(productResult.Data.Where(x=>x.IsVariationOverlay).ToList(), "Id", "Name");

            return PartialView(new CreateVariation());
        }
        // POST: Categories/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateVariation model)
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
            });

        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return Json(new
                {
                    status = false,
                    message = "Invalid Id"
                }, JsonRequestBehavior.AllowGet);
            }
            var result = await _entityService.Edit(id);

            var productResult = await _productService.List();

            ViewBag.ColorId = new SelectList(productResult.Data.Where(x => x.IsHasVariation).ToList(), "Id", "Name", result.Data.ColorId);
            ViewBag.ItemId = new SelectList(productResult.Data.Where(x => x.IsVariationOverlay).ToList(), "Id", "Name", result.Data.ItemId);

            return PartialView(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditVariation model)
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


        // POST: Products/Upload
        public ActionResult Upload(HttpPostedFileBase Image)
        {
            int sizeInBytes = 1024 * 1024 * 4; //4mb
            string permittedType = "image/jpeg,image/gif,image/png";

            try
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    bool dirExists = Directory.Exists(Server.MapPath(Config.CustomProducts));

                    if (!dirExists)
                    {
                        Directory.CreateDirectory(Server.MapPath(Config.CustomProducts));
                    }

                    var fileName = Guid.NewGuid().ToString();
                    string path = Path.Combine(Server.MapPath(Config.CustomProducts), fileName + ".jpg");

                    if (Image.ContentLength > sizeInBytes)
                    {
                        return Json(new
                        {
                            status = false,
                            message = Language.Language.Image_size_cannot_be_more_than_4MB,
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (!permittedType.Split(",".ToCharArray()).Contains(Image.ContentType))
                        {
                            return Json(new
                            {
                                status = false,
                                message = Language.Language.Please_upload_valid_image_file_like_jpeg_or_png,
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var flag = true;

                            if (System.IO.File.Exists(path))
                            {
                                while (flag)
                                {
                                    fileName = Guid.NewGuid().ToString();
                                    path = Path.Combine(Server.MapPath(Config.CustomProducts), fileName + ".jpg");
                                    flag = false;
                                }
                            }

                            Image.SaveAs(path);
                        }
                    }

                    return Json(new
                    {
                        status = true,
                        message = Language.Language.Upload,
                        file = fileName + ".jpg"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        message = Language.Language.No_image_was_found
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.InnerException.Message
                }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}