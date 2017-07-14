using Library.Web.Code;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Areas.Members.Controllers {
    [Area(SiteAreas.Members)]
    [Authorize(Policy="MemberOnly")]
    public class HomeController: Controller {
        public IActionResult Index() {
            return View();
        }
    }
}