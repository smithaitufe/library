using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Areas.Control.Librarian
{
    [Area("Librarian")]
    public class HomeController: Controller {

        public IActionResult Welcome()    {
            return View();
        }
    }
}