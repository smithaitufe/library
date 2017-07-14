using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Library.Core.Models;
using Library.Web.Models.AccountViewModels;
using Library.Web.Code;
using Library.Repo;
using Library.Web.Services;

using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;

namespace Library.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;        
        private readonly LibraryDbContext _context;

        public AccountController(
            RoleManager<Role> roleManager,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            LibraryDbContext context,            
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;            
            _logger = loggerFactory.CreateLogger<AccountController>();
        }
        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var model = new RegisterViewModel();
            PopulateDropdowns(model);
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.PhoneNumber, Email = model.PhoneNumber, FirstName = model.FirstName, LastName = model.LastName };
                // Add user claim
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Role,Code.Roles.Member,ClaimValueTypes.String),
                    new Claim(ClaimTypes.GivenName,model.PhoneNumber, ClaimValueTypes.String)
                };               
                
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Member);
                    await _userManager.AddClaimsAsync(user, claims);
                    var userAddress = new UserAddress { User = user, Address = new Address { Line = model.Line, City = model.City, StateId = model.StateId } };
                    _context.UserAddresses.Add(userAddress);
                    await _context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    model.RegistrationSuccessful = true;
                    return View(model);

                    
                }
                AddErrors(result);
            }
            PopulateDropdowns(model);
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.PhoneNumber);
                if (user == null)
                {
                    ModelState.AddModelError("Message", "There is no user with the Phone Number you specified");
                    return View(model);
                }
                var resetPasswordModel = new ResetPasswordViewModel();
                resetPasswordModel.PhoneNumber = user.PhoneNumber;
                //Send code to mobile phone
                // resetPasswordModel.Code = "";
                return View("ForgotPasswordConfirmation", resetPasswordModel);
            }
            return View(model);
        }


        [HttpGet]        
        public IActionResult ChangePassword(string returnUrl) {
            return View(new ChangePasswordViewModel());
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model) {
            
            if(!ModelState.IsValid) {
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if(!result.Succeeded){
                ModelState.AddModelError("OldPassword", "The current password you entered is invalid");
                return View(model);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home", new { area = ""});
            
        }
        
        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.PasswordChanged = true;
                    return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
                }
                AddErrors(result);
            }
            return View();
        }
        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            if (ViewBag.PasswordChanged == null || !ViewBag.PasswordChanged)
                return NotFound();
            return View();
        }
        //
        // GET /Account/AccessDenied
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        private void PopulateDropdowns(RegisterViewModel model)
        {
            model.States = _context.States.OrderBy(s => s.Name).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();
        }

    }

}
