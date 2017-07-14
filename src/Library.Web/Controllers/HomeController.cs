using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Library.Core.Models;
using Library.Web.Areas.Members.Controllers;
using Library.Web.Code;
using Library.Web.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

namespace Library.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;        
        private readonly ILogger _logger;

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;            
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<HomeController>();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl = null)
        {
            await HttpContext.SignOutAsync("Cookies");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                
                var user = _userManager.Users.Where(u => u.PhoneNumber == model.ID || u.LibraryNo == model.ID).SingleOrDefault();
                if (user != null)
                {
                    
                    if (user.Approved)
                    {
                        var valid = await _userManager.CheckPasswordAsync(user, model.Password);
                        if (valid)
                        {

                            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                            if (user.ChangePasswordFirstTimeLogin)
                            {
                                var changePasswordModel = new ChangePasswordFirstTimeLoginViewModel();
                                changePasswordModel.UserName = user.UserName;
                                changePasswordModel.Role = role;
                                changePasswordModel.OldPassword = model.Password;
                                return View("ChangePasswordFirstTimeLogin", changePasswordModel);
                            }

                            var claims = new List<Claim> { new Claim(ClaimTypes.Role, role, ClaimValueTypes.String) };
                            claims = GetTokenClaims(user).Union(claims).ToList();
                            var id = new ClaimsIdentity(claims.ToList(), "Password");
                            var principal = new ClaimsPrincipal(id);
                            await HttpContext.SignInAsync("Cookies", principal, new AuthenticationProperties
                            {
                                IsPersistent = model.RememberMe,
                                ExpiresUtc = System.DateTime.UtcNow.AddMinutes(20),
                                AllowRefresh = false
                            });
                            _logger.LogInformation(1, "User logged in.");
                            return RedirectToLocal(returnUrl, role);
                        }
                        else
                        {
                            ModelState.AddModelError("LoginError", "The password you entered is incorrect. Try again!");
                            return View(model);
                        }                        
                    }
                    else
                    {
                        ModelState.AddModelError("LoginError", "Your account is under review. Check back later");
                        return View(model);
                    }
                }
                else
                {

                    ModelState.AddModelError("LoginError", "The Library ID or Phone Number was not found");
                    return View(model);
                }

            }
            return View(nameof(HomeController.Index));
        }

        [HttpPost]

        public async Task<IActionResult> ChangePasswordFirstTimeLogin(ChangePasswordFirstTimeLoginViewModel model)
        {
            if(!ModelState.IsValid) {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            return RedirectToLocal(null, model.Role);
        }
        public IActionResult Forbidden()
        {
            return View();
        }
        private IActionResult RedirectToLocal(string returnUrl, string role)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                switch (role)
                {
                    case "SuperAdministrator":
                        return RedirectToAction(nameof(HomeController.Index), "Home", new { area = SiteAreas.SuperAdministrator });
                    case "Administrator":
                        return RedirectToAction(nameof(DashboardController.Index), "Dashboard", new { area = SiteAreas.Admin });
                    default:
                        return RedirectToAction(nameof(DashboardController.Index), "Dashboard", new { area = SiteAreas.Members });
                }

            }
        }
        //
        // POST: /Session/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            //await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync("Cookies");
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index));
        }
        private IEnumerable<Claim> GetTokenClaims(User user)
        {
            return new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String),
                new Claim(ClaimTypes.GivenName, user.FirstName, ClaimValueTypes.String)
            };
        }

    }
}