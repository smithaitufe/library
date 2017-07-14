using System;
using System.Linq;
using System.Threading.Tasks;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Models.DashboardViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Areas.Control.Admin.Controllers
{
    [Area(SiteAreas.Admin)]
    [Authorize(Policy="AdministratorOnly")]
    public class DashboardController: Controller {
        LibraryDbContext _context;
        private readonly BookService _bookService;

        public DashboardController(LibraryDbContext context) {
            _context = context;
            _bookService = new BookService(context);
        }
        public async Task<IActionResult> Index() {
            var model = new AdminDashboardIndexViewModel();

            var checkOutQuery = _bookService.GetCheckOutBooks()
            .Where(c => c.CheckOutStates.OrderByDescending(cs=>cs.Id)
                .Take(1)
                .Where(cs=>cs.Status.Name.ToLower().Equals("borrow approved"))
            .Any());

            var checkOuts = await checkOutQuery.ToListAsync();
            foreach (var checkOut in checkOuts)
            {
                var numberOfDays = int.Parse(checkOut.ApprovedDays.Name);
                
            }
            var pendingCheckouts = checkOutQuery.Where(c => c.CheckOutStates
                .Take(1)
                .Where(cs=>cs.Status.Name.ToLower().Equals("borrow approved"))
            .Any());






            await PopulateLocationDropdown();
            model.TotalRegistrations = _context.Users.Where(u => u.Approved == false).Count();
            return View(model);
        }


        public IActionResult GetBookSummary(int locationId, Nullable<DateTime> date)
        {
            
            var books = _bookService.GetAllVariants().Where(v => v.VariantCopies.Where(vl=>vl.LocationId == locationId).Any());            
            var totalBooksOnShelfQuery = books.Where(b => b.VariantCopies.Where(vl=>vl.Availability.Name.Equals("On Shelf")).Any());
            var totalPendingCheckoutBooksQuery = books.Where(b => b.CheckOuts.Where(c=> c.CheckOutStates.OrderByDescending(cs=>cs.Id).Take(1).Where(cs=>cs.Status.Name.ToLower().Equals("borrow initiated")).Any()).Any());
            var totalReturnedCheckoutBooksQuery = books.Where(b => b.CheckOuts.Where(c=> c.CheckOutStates.OrderByDescending(cs=>cs.Id).Take(1).Where(cs=>cs.Status.Name.ToLower().Equals("return initiated")).Any()).Any());

            if(date.HasValue)
            {
                DateTime.TryParse(date.Value.ToString(), out var result);
                totalBooksOnShelfQuery = totalBooksOnShelfQuery.Where(b => b.VariantCopies.Where(vl=>vl.InsertedAt <= result).Any());
            }

            var totalBooksOnShelf = totalBooksOnShelfQuery.Count();
            var totalPendingCheckoutBooks = totalPendingCheckoutBooksQuery.Count();
            var totalReturnedCheckoutBooks = totalReturnedCheckoutBooksQuery.Count();

            var bookStatistics = new BookLocationStatisticsViewModel(){ 
                TotalBooksOnShelf = totalBooksOnShelf, 
                TotalBooksCheckedOut = totalPendingCheckoutBooks,
                TotalBooksReturned = totalReturnedCheckoutBooks
            };

            //var totalPendingCheckoutBooks = _bookService.GetCheckOutBooks().Where(c=>c.Returned == false).Count();

            return PartialView("_LocationStatisticsPartial", bookStatistics);
        }


        private async Task PopulateLocationDropdown()
        {
            var locations = await _context.Locations.OrderBy(l=>l.Name).ToListAsync();
            ViewBag.Locations = locations;
        }
    }
}