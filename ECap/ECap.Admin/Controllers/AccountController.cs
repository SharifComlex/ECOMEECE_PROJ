using ECap.Admin.Models;
using ECap.Core.Database;
using ECap.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECap.Admin.Controllers
{
    public class AccountController : Controller
    {
        private ECapDbContext _dbContext;
        private readonly UserManager<AdminUser> _userManager;
        private readonly SignInManager<AdminUser> _signInManager;

        public AccountController(ECapDbContext dbContext, UserManager<AdminUser> userManager, SignInManager<AdminUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return PartialView(new LoginViewModel());
        }

        //
        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PartialView(model);
            }


            try
            {
                using (_dbContext)
                {
                    var user = await _dbContext
                    .Users
                    .Select(x => new
                    {
                        x.Email,
                        x.Status,
                        x.Role
                    })
                    .SingleOrDefaultAsync(x => x.Email == model.Email);

                    if (user == null)
                    {
                        ModelState.AddModelError("Password", "Invalid login attempt.");
                        return PartialView(model);
                    }

                    if (!user.Status)
                    {
                        ModelState.AddModelError("Password", "Invalid login attempt.");
                        return PartialView(model);
                    }

                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, change to shouldLockout: true
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        return Redirect("/");
                    }

                    ModelState.AddModelError("Password", "Invalid login attempt.");
                    return PartialView(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Password", "Invalid login attempt.");
                return PartialView(model);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Password()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Password(ChangePassword model)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == UserId);

            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (result.Succeeded) {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("OldPassword", item.Description);
                }
            }

            return View(model);

        }

        // POST: /Account/logout
        [Authorize]
        public async Task<IActionResult> logout()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

        }
    }
}