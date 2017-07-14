using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Extensions;
using Library.Web.Models.AccountViewModels;
using Library.Web.Models.CommonPageSettingsViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Library.Web.Areas.Admin.Controllers
{
    [Area(SiteAreas.Admin)]
    [Authorize(Policy="AdministratorOnly")]    
    public class AccountController: Controller {
        private readonly LibraryDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IdentityService identityService;
        private readonly SitePageSettingsViewModel _pageSettings;

        public AccountController(LibraryDbContext context,  UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<SitePageSettingsViewModel> pageSettings) {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            identityService = new IdentityService(context, userManager, roleManager);
            _pageSettings = pageSettings.Value;
        }
        public IActionResult Index(string role) {
            var userListing = new UserListingViewModel();
            var query = from u in identityService.GetAllUsers() select u;

            // //Working with claims
            // var users = _userManager.Users.Select(u => u).ToList();
            // var claim = new Claim(ClaimTypes.Surname, users.SingleOrDefault().FirstName);
            // var result = await _userManager.AddClaimAsync(users.First(), claim);

            if(!string.IsNullOrEmpty(role)){
                query = from u in query join ur in _context.UserRoles
                    on u.Id equals ur.UserId
                    join r in identityService.GetAllRoles()
                    on ur.RoleId equals r.Id
                    where  r.Name == role
                    select u;
            }                         
            userListing.Users = query.MapToUserViewModel().ToList();
            userListing.Roles = identityService.GetAllRoles().MapToRoleViewModel().ToList();            
            return View(userListing);
        }

        public async Task<IActionResult> Create() {
            var model = new CreateUserViewModel(); 
            await PopulateDropdownItems(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model) {
            if(!ModelState.IsValid) {  
                await PopulateDropdownItems(model);
                return View(model);                
            }
            var user = model.MapToUser();  
            user.LocationsLink.Add(new UserLocation { LocationId = model.LocationId});
            var claim = new Claim(ClaimTypes.Role, model.Role,ClaimValueTypes.String);            
            var identityService = new IdentityService(_context, _userManager);
            var result = await identityService.AddUser(user, model.Password);   
            await _userManager.AddClaimAsync(user, claim);             
            if(result.Succeeded) {
                await _userManager.AddToRoleAsync(user, model.Role);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id) {            
            var identityService = new IdentityService(_context, _userManager, _roleManager);
            var model = identityService.GetAllUsers().Where(u => u.Id == id).MapToUserViewModel().SingleOrDefault();
            model.Roles = identityService.GetUserRoles(id).MapToRoleViewModel().ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult Registrations(FilterUserRegistration FilterUserRegistrationData) {
            
            var query = from u in _userManager.Users.Where(u => u.Approved == false) select u;
            if(FilterUserRegistrationData.StartDate.HasValue && FilterUserRegistrationData.EndDate.HasValue){
                query = query.Where(ur => ur.InsertedAt >= FilterUserRegistrationData.StartDate.Value && ur.InsertedAt <= FilterUserRegistrationData.EndDate.Value);
            }
            else if(FilterUserRegistrationData.StartDate.HasValue && !FilterUserRegistrationData.EndDate.HasValue){
                query = query.Where(ur => ur.InsertedAt >= FilterUserRegistrationData.StartDate.Value);
            }
            else if(!FilterUserRegistrationData.StartDate.HasValue && FilterUserRegistrationData.EndDate.HasValue){
                query = query.Where(ur => ur.InsertedAt <= FilterUserRegistrationData.EndDate.Value);
            }
            
            var registrations = query.Select(u => new UserRegistrationViewModel {
                UserId = u.Id,
                LastName = u.LastName,
                FirstName = u.FirstName,
                Email = u.Email,
                InsertedAt = u.InsertedAt,
            }).ToList();

            var userListing = new UserRegistrationListingViewModel();
            userListing.Registrations = registrations;
            return View(userListing);
        }
        [HttpPost]
        public async Task<IActionResult> Registrations(FilterUserRegistration FilterUserRegistrationData, UserRegistrationListingViewModel model) {
            User user = null;
            foreach (var registration in model.Registrations) {
                user = _userManager.Users.SingleOrDefault(u => u.Id == registration.UserId);
                if(user != null){
                    //Generate library no
                    if(string.IsNullOrEmpty(user.LibraryNo)) user.LibraryNo =  $"{_pageSettings.Code}/{Common.RandomNumbers(8)}";
                    user.Approved = registration.Approved;
                    _context.Entry(user).State = EntityState.Modified;
                }
            }            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AccountController.Index));
        }

        private async Task PopulateDropdownItems(CreateUserViewModel model) {
            var termService = new TermService(_context);
            model.Roles = await _context.Roles.Select(r => new SelectListItem{ Value = r.Name, Text = r.Name}).ToListAsync();
            model.Roles = identityService.GetAllRoles().Select(r => new SelectListItem{ Value = r.Name, Text = r.Name}).ToList();
            model.Locations = _context.Locations.MapToSelectList().ToList();
            // model.UserClaims = ClaimsData.UserClaims.Select(c => new SelectListItem
            // {
            //     Text = c.Claim,
            //     Value = c.Claim
            // }).ToList();
        }
        private void PopulateDropdownItems(UserListingViewModel model) {
            model.Roles = identityService.GetAllRoles().MapToRoleViewModel().ToList();
        }
    }
}