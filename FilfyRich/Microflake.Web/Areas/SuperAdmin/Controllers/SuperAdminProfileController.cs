using Microflake.Core;
using Microflake.Core.Persistence;
using Microflake.Core.ViewModel.Profile;
using Microflake.Web.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microflake.Web.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminProfileController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        [DefaultConstructor]
        public SuperAdminProfileController()
        {
        }

        //[DefaultConstructor]
        //public SuperAdminProfileController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
        //}

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: SuperAdmin/SuperAdminProfile
        public ActionResult Index()
        {
            var UserId = User.Identity.GetUserId();
            using (var _dbContext = new ApplicationDbContext())
            {
                var user = _dbContext.Users
                    .Select(x => new
                    {
                        x.Id,
                        x.UserName,
                        x.PhoneNumber,
                        x.Email,
                       
                    }).ToList()
                    
                    .Where(x => x.Id == UserId)
                    .FirstOrDefault();

                return View(new SuperAdminProfileViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                   
                    Email = string.IsNullOrEmpty(user.Email) ? "" : user.Email.Substring(1),
                    PhoneNumber = user.PhoneNumber,
                   
                  
                });
            }
           
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePassword(ChangePasswordViewModel model)
        {
            using (var _dbContext = new ApplicationDbContext())
            {
                var UserId = User.Identity.GetUserId();

                var user = _dbContext.Users
                    .Where(x => x.Id == UserId)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(model.OldPassword))
                {
                    var result = (new PasswordHasher()).VerifyHashedPassword(user.PasswordHash, model.OldPassword) == PasswordVerificationResult.Success;

                    if (!result)
                    {
                        ModelState.AddModelError("OldPassword", Language.Language.Old_password_did_not_match_);
                    }
                }


                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        status = false,
                        validation = false,
                        message =Language.Language.Provide_All_Required_Inputs,
                        errors = ModelState.Where(ms => ms.Value.Errors.Any())
                                            .Select(x => new {
                                                x.Key,
                                                Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                                            })
                    }
                    , JsonRequestBehavior.AllowGet);
                }

                var hashpassword = (new PasswordHasher()).HashPassword(model.NewPassword);

                user.PasswordHash = hashpassword;

                _dbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
                var passwordResult = await _dbContext.SaveChangesAsync();

                if (passwordResult > 0)
                {
                    return Json(new
                    {
                        status = true,
                        message = Language.Language.Password_has_been_changed
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    status = false,
                    message = Language.Language.Unable_to_change_the_password
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Upload(HttpPostedFileBase Profile)
        {
            int sizeInBytes = 1024 * 1024 * 4; //4mb
            string permittedType = "image/jpeg,image/gif,image/png";

            try
            {
                if (Profile != null && Profile.ContentLength > 0)
                {
                    bool dirExists = Directory.Exists(Server.MapPath(Config.Profile));

                    if (!dirExists)
                    {
                        Directory.CreateDirectory(Server.MapPath(Config.Profile));
                    }

                    var fileName = User.Identity.GetUserId();
                    string path = Path.Combine(Server.MapPath(Config.Profile), fileName + ".jpg");

                    if (Profile.ContentLength > sizeInBytes)
                    {
                        return Json(new
                        {
                            status = false,
                            message = Language.Language.Image_size_cannot_be_more_than_4MB,
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (!permittedType.Split(",".ToCharArray()).Contains(Profile.ContentType))
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
                                    fileName = User.Identity.GetUserId();
                                    //fileName = Guid.NewGuid().ToString();
                                    path = Path.Combine(Server.MapPath(Config.Profile), fileName + ".jpg");
                                    flag = false;
                                }
                            }

                            Profile.SaveAs(path);
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