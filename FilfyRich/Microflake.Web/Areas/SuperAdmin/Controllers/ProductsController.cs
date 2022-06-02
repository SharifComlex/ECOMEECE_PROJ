﻿
using Microflake.Core;
using Microflake.Core.Application.Categories;
using Microflake.Core.Application.Products;
using Microflake.Core.Application.SubCategories;
using Microflake.Core.ViewModel.Products;
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
    public class ProductsController : BaseController
    {
        private readonly IProductService _entityService;

        private readonly ICategoryService _CategoryService;
        private readonly ISubCategoryService _SubCategoryService;

        public ProductsController(ISubCategoryService SubCategoryService ,ICategoryService CategoryService,IProductService entityService)
        {
            _entityService = entityService;
            _SubCategoryService = SubCategoryService;
            _CategoryService = CategoryService;

           
        }
        // GET: SuperAdmin/Products
        // GET: SuperAdmin/AttributeGroups
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
            var SubcategoryResult = await _SubCategoryService.List();
            //ViewBag.SubCategoryId = new SelectList(categoryResult.Data, "Id", "Name");

            if (_language)
            {
                ViewBag.SubCategoryId = new MultiSelectList(SubcategoryResult.Data.ToList(), "Id", "Arabic");
            }
            else
            {
                ViewBag.SubCategoryId = new MultiSelectList(SubcategoryResult.Data.ToList(), "Id", "English");
            }


            return PartialView(new CreateProduct());
        }
        // POST: Categories/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateProduct model)
        {
            if (model.Name == null)
            {
                ModelState.AddModelError("English", Language.Language.This_Field_is_Required);

            }

            if (model.Price > model.SellPrice)
            {
                ModelState.AddModelError("SellPrice", Language.Language.Selling_Price_Should_Be_greater_than_Purchase);

            }
            if (model.Discount < model.SellPrice || model.Discount < model.Price)
            {
                ModelState.AddModelError("Discount", Language.Language.Enter_Discount_As_Compare_to_Original_Price);

            }
            if (model.Qty == 0 ||model.Qty < 0 )
            {
                ModelState.AddModelError("Qty", Language.Language.Enter_A_Valid_Quantity);

            }
            if (model.Price == 0 || model.Price < 0)
            {
                ModelState.AddModelError("Price",  Language.Language.Enter_A_Valid_Price);

            }
            if (model.SellPrice == 0 || model.SellPrice < 0)
            {
                ModelState.AddModelError("SellPrice", Language.Language.Enter_A_Valid_Price);

            }
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

            var SubcategoryResult = await _SubCategoryService.List();
            //ViewBag.SubCategoryId = new SelectList(categoryResult.Data, "Id", "Name");

            if (_language)
            {
                ViewBag.SubCategoryId = new MultiSelectList(SubcategoryResult.Data.ToList(), "Id", "Arabic");
            }
            else
            {
                ViewBag.SubCategoryId = new MultiSelectList(SubcategoryResult.Data.ToList(), "Id", "English");
            }
            return PartialView(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditProduct model)
        {

            if (model.Name == null)
            {
                ModelState.AddModelError("English", Language.Language.This_Field_is_Required);

            }

            if (model.Price > model.SellPrice)
            {
                ModelState.AddModelError("SellPrice", Language.Language.Selling_Price_Should_Be_greater_than_Purchase);

            }
            if (model.Discount < model.SellPrice || model.Discount < model.Price)
            {
                ModelState.AddModelError("Discount", Language.Language.Enter_Discount_As_Compare_to_Original_Price);

            }
            if (model.Qty == 0 || model.Qty < 0)
            {
                ModelState.AddModelError("Qty", Language.Language.Enter_A_Valid_Quantity);

            }
            if (model.Price == 0 || model.Price < 0)
            {
                ModelState.AddModelError("Price", Language.Language.Enter_A_Valid_Price);

            }
            if (model.SellPrice == 0 || model.SellPrice < 0)
            {
                ModelState.AddModelError("SellPrice", Language.Language.Enter_A_Valid_Price);

            }
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
        public ActionResult Upload(HttpPostedFileBase Product)
        {
            int sizeInBytes = 1024 * 1024 * 4; //4mb
            string permittedType = "image/jpeg,image/gif,image/png";

            try
            {
                if (Product != null && Product.ContentLength > 0)
                {
                    bool dirExists = Directory.Exists(Server.MapPath(Config.Products));

                    if (!dirExists)
                    {
                        Directory.CreateDirectory(Server.MapPath(Config.Products));
                    }

                    var fileName = Guid.NewGuid().ToString();
                    string path = Path.Combine(Server.MapPath(Config.Products), fileName + ".jpg");

                    if (Product.ContentLength > sizeInBytes)
                    {
                        return Json(new
                        {
                            status = false,
                            message = Language.Language.Image_size_cannot_be_more_than_4MB,
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (!permittedType.Split(",".ToCharArray()).Contains(Product.ContentType))
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
                                    path = Path.Combine(Server.MapPath(Config.Products), fileName + ".jpg");
                                    flag = false;
                                }
                            }

                            Product.SaveAs(path);
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