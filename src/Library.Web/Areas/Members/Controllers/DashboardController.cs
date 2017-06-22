using System.Linq;
using System.Security.Claims;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Models.DashboardViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Areas.Members.Controllers
{
    [Area(SiteAreas.Members)]
    [Authorize(Policy="MemberOnly")]
    public class DashboardController: Controller
    {
        LibraryDbContext _context;
        PostService postService;
        BookService bookService;    
        AnnouncementService announcementService;
        IdentityService identityService;
        public DashboardController(LibraryDbContext context, UserManager<User> _userManager) {
            _context = context;
            postService = new PostService(_context);
            bookService = new BookService(_context);
            announcementService = new AnnouncementService(_context);
            identityService = new IdentityService(_context, _userManager);
            
        }
        public IActionResult Index() {
            var statistics = new DashboardIndexViewModel();
            statistics.Posts = postService.GetAllPosts().OrderByDescending(p=>p.Id).Take(3).ToList();
            statistics.TotalBooksBorrowed = bookService.GetCheckOutBooks("Approved").Where(c =>  c.PatronId == (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))).Count();
            statistics.TotalBooksReturned = bookService.GetCheckOutBooks("Return Confirmed").Where(c =>  c.PatronId == (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))).Count();
            statistics.Announcements = announcementService.GetAll().ToList();
            statistics.TotalMembers = identityService.GetAllUsers().Count();
            return View(statistics);
        }
    }
}