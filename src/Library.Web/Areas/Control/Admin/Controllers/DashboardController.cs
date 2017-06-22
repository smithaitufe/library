using System.Linq;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Models.DashboardViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Areas.Control.Admin.Controllers
{
    [Area(SiteAreas.Admin)]
    [Authorize(Policy="AdministratorOnly")]
    public class DashboardController: Controller {
        LibraryDbContext _context;
        public DashboardController(LibraryDbContext context) {
            _context = context;
        }
        public IActionResult Index() {
            var model = new AdminDashboardIndexViewModel();
            model.TotalRegistrations = _context.Users.Where(u => u.Approved == false).Count();
            return View(model);
        }
    }
}