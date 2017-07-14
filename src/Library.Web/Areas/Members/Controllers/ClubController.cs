using System.Linq;
using System.Security.Claims;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Extensions;
using Library.Web.Models.ClubViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Areas.Members.Controllers {
    [Area(SiteAreas.Members)]
    [Authorize(Policy="MemberOnly")]
    public class ClubController: Controller {
        private readonly LibraryDbContext context;        
        int userId;
        public ClubController(LibraryDbContext context, IHttpContextAccessor httpContextAccessor) {
            this.context = context;            
            userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        public IActionResult Index() {            
            var clubService = new ClubService(context);            
            var clubs = clubService.GetMemberClubs(userId).MapToClubViewModel();
            return View(clubs);
        }
        // [Route("{id:int}")]
        public IActionResult Details(int id) {
            var clubService = new ClubService(context);            
            var club = clubService.GetMemberClubs(userId).Where(c => c.Id == id).MapToClubViewModel().SingleOrDefault();
            return View(club);
        }
        [HttpGet]
        public IActionResult Join() {
            var clubService = new ClubService(context);
            var model = new JoinClubViewModel(userId);            
            var query = clubService.GetAllClubs();
            model.Clubs = query.MapToClubViewModel().ToList();
            return PartialView("~/Areas/Members/Views/Club/_Join.cshtml", model);
        }
        [HttpPost]
        public IActionResult Join(JoinClubViewModel model) {
            if(ModelState.IsValid){
                context.Set<ClubMember>().Add(new ClubMember { UserId = model.UserId, ClubId = model.ClubId  });
                context.SaveChanges();
                return RedirectToAction(nameof(ClubController.Index));
            }
            ModelState.AddModelError("Failed", "The operation was unsuccessful. Try again");
            return PartialView("~/Areas/Members/Views/Club/_Join.cshtml", model);
            
        }
    }
}