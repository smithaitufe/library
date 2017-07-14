using Library.Repo;
using Library.Web.Code;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Areas.Control.SuperAdministrator {
    [Area(SiteAreas.SuperAdministrator)]
    [Authorize(Policy="SuperAdministratorOnly")]
    public class HomeController: Controller {
        private readonly LibraryDbContext _context;
        
        public HomeController(LibraryDbContext context){
            _context = context;
        }

        public IActionResult Index() {
            return View();
        }
    }
}