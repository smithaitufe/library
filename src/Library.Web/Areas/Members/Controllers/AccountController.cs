using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Extensions;
using Library.Web.Models.AccountViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Areas.Members.Controllers
{
    [Area(SiteAreas.Members)]
    [Authorize(Policy="MemberOnly")]
    public class AccountController: Controller{
        private int userId;
        private readonly LibraryDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public AccountController(LibraryDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager){            
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index() {
            var profile = new PatronProfileViewModel();
            userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);            
            var user = _userManager.Users
            .Include(u => u.Posts)
            .Include(u => u.Comments)
            .Include(u=>u.AddressesLink)
            .ThenInclude(al=>al.Address)
            .ThenInclude(a => a.State)
            .Where(u => u.Id == userId).SingleOrDefault();

            profile.Name = $"{user.LastName} {user.FirstName}";
            profile.PhoneNumber = user.PhoneNumber;
            profile.Roles = await _userManager.GetRolesAsync(user);
            profile.Address = user.AddressesLink.Select(al => al.Address).FirstOrDefault();
            profile.NumberOfPosts = user.Posts.Count();
            profile.NumberOfComments = user.Comments.Count();
            profile.LibraryNo = user.LibraryNo;
            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> Edit() {
            var profile = new EditPatronProfileViewModel();
            userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);            
            var user = _userManager.Users
            .Include(u => u.Posts)
            .Include(u => u.Comments)
            .Include(u=>u.AddressesLink)
            .ThenInclude(al=>al.Address)
            .ThenInclude(a => a.State)
            .Where(u => u.Id == userId).SingleOrDefault();

            profile.FirstName = user.FirstName;
            profile.LastName = user.LastName;
            profile.PhoneNumber = user.PhoneNumber;

            var address = user.AddressesLink.Select(al => al.Address).SingleOrDefault();
            if(address != null){
                profile.Address.Line = address.Line;
                profile.Address.City = address.City;
                profile.Address.StateId = address.StateId;
            }
            await PopulateDropdowns(profile);
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPatronProfileViewModel model) {
            
            if(!ModelState.IsValid) {
                return View(model);
            }
            _context.UserAddresses.Add(new UserAddress{});
            
            await PopulateDropdowns(model);
            return RedirectToAction(nameof(AccountController.Index));
        }
        private async Task PopulateDropdowns(EditPatronProfileViewModel model) {
            var termService = new TermService(_context);
            model.Address.States = await Task.Run(() => _context.States.OrderBy(s => s.Name).MapToSelectList().ToList()); 
        }
    }
}