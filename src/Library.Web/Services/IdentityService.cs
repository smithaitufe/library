using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Core.Models;
using Library.Data;
using Library.Models;
using Library.Models.AccountViewModels;
using Library.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Services
{
    public class IdentityService {
        private LibraryDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public IdentityService(LibraryDbContext context, UserManager<User> userManager) {
            _context = context;
            _userManager = userManager;
        }
        public IdentityService(LibraryDbContext context, RoleManager<Role> roleManager) {
            _context = context;
            _roleManager = roleManager;
        }
        public IdentityService(LibraryDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager) {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        private IQueryable<User> Users => _context.Users
            .Include(u => u.Roles)
            .Include(u => u.LocationsLink)
            .ThenInclude( ll => ll.Location);

        public IQueryable<User> GetAllUsers () {
            return Users;
        }
        public async Task<UserViewModel> GetUserById(int userId) {
            var user = await Users.Where(u => u.Id == userId).FirstOrDefaultAsync();            
            var userRoles = _userManager.GetRolesAsync(user).Result.ToList();            
            var role = _roleManager.Roles.Where(r => userRoles.Contains(r.Name)).SingleOrDefault() ?? null;
            return new UserViewModel {
                UserId = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Locations = user.LocationsLink.ToList()         
            };            
        }
        public async Task<IdentityResult> AddUser(User user, string password) {
            return await _userManager.CreateAsync(user, password);
        }
        public async void AddUserToLocation(int userId, int locationId) {
            var location = new UserLocation { UserId = userId, LocationId = locationId};
            _context.UserLocations.Add(location);
            await _context.SaveChangesAsync();
        }
        public async void ToggleUserSuspension(int userId) {
            var user = _context.Users.Find(userId);
            user.Suspended = !user.Suspended;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public IQueryable<Role> GetAllRoles () {
            return _context.Roles;
        }
        public IQueryable<Role> GetUserRoles (int userId) {
            return  from r in GetAllRoles()
                    join ur in _context.UserRoles
                    on r.Id equals ur.RoleId
                    where  ur.UserId == userId
                    select r;
        }



    }
}