using Library.Core.Models;
using Library.Web.Code;
using Library.Web.Extensions;
using Library.Web.Models.BookViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Library.Repo;

namespace Library.Web.Areas.Control.Admin.Controllers
{
    [Area(SiteAreas.Admin)]
    [Authorize(Policy="AdministratorOnly")]
    public class CirculationController: Controller {
        private readonly LibraryDbContext _context;
        private readonly BookService bookService;
        private readonly TermService termService;
        public CirculationController(LibraryDbContext context) {
            _context = context;
            bookService = new BookService(context);
            termService = new TermService(context);
        }

        public async Task<IActionResult> Index(BookCirculationSearchViewModel SearchCirculationsOptions) {
            var bookCirculationListing = new BookCirculationListingViewModel();
            await PopulateDropdowns(bookCirculationListing);
            if(!string.IsNullOrEmpty(SearchCirculationsOptions.Status)){
                var query = bookService.GetCheckOutBooks().Where(c => c.CheckOutStates.OrderByDescending(cs=>cs.Id).Take(1).Where(cs => cs.Status.Name.ToLower().Equals(SearchCirculationsOptions.Status.ToLower())).Any());
                if(SearchCirculationsOptions.StartDate.HasValue && SearchCirculationsOptions.EndDate.HasValue) {
                    query = query.Where(c => c.InsertedAt.Date >= SearchCirculationsOptions.StartDate.Value.Date && SearchCirculationsOptions.EndDate.Value <= c.InsertedAt.Date);
                }
                else if(SearchCirculationsOptions.StartDate.HasValue && !SearchCirculationsOptions.EndDate.HasValue) {
                    query = query.Where(c => c.InsertedAt.Date >= SearchCirculationsOptions.StartDate.Value.Date);
                }
                else if(!SearchCirculationsOptions.StartDate.HasValue && SearchCirculationsOptions.EndDate.HasValue) {
                    query = query.Where(c => c.InsertedAt.Date <= SearchCirculationsOptions.EndDate.Value.Date);
                }
                var ctions = query.ToList();
                var circulations = query.MapToBookCirculationViewModel().ToList();
                bookCirculationListing.SearchCirculationsOptions.Status = SearchCirculationsOptions.Status;
                bookCirculationListing.Circulations = circulations;
            }            
            return View(bookCirculationListing);
        }
        [HttpPost]
        public async Task<IActionResult> Index(BookCirculationSearchViewModel SearchCirculationsOptions, BookCirculationListingViewModel model) {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var approvedStateId = _context.CheckOutStatuses.SingleOrDefault(cos => cos.Name.ToLower().Equals("approved")).Id;
            foreach(var c in model.Circulations) {                
                if(c.StatusId.HasValue){
                    var state = new CheckOutState { CheckOutId = c.CheckOutId, StatusId = c.StatusId.Value, ModifiedByUserId = userId };
                    _context.CheckOutStates.Add(state);
                    await _context.SaveChangesAsync();
                    if(c.StatusId.Value == approvedStateId) {
                        var checkOut = bookService.GetCheckOutBooks().Where(co => co.Id == c.CheckOutId).SingleOrDefault();
                        checkOut.ApprovedDaysId = c.ApprovedDaysId;
                        checkOut.UpdatedAt = System.DateTime.Now;                        
                        _context.Entry(checkOut).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            
            
            return View();
        }
        public IActionResult Manage(string type) {
            switch (type.ToLower()) {
                case "return": 
                    return View();
                case "reserve":
                    return View();
                default:
                break;
            }   
            return View();
        }
        [HttpGet]
        public IActionResult ReserveBook() {
            ViewBag.SerialNo = Common.RandomString();
            ViewBag.SerialNo2 = Common.RandomKeys();
            var model = new ReserveBookFormViewModel();
            PopulateDropdowns(model);
            return View(model);
        }
        [HttpPost]
        public IActionResult ReserveBook(ReserveBookFormViewModel model) {
            if(!ModelState.IsValid){
                PopulateDropdowns(model);
                return View(model);
            }
            return View(model);
        }
        public IActionResult GetPatronByNo(string no) {
            var patron = _context.Users.SingleOrDefault(u => u.UserName.ToLower().Equals(no.ToLower()));
            if(patron == null) return Json(new {successful = false, message= $"No patron with Library No {no} was found"});
            return Json(new {successful= true, patron = patron});
        }
        public IActionResult GetBookBySerialNo(string serialNo) {
            var book = bookService.GetAllVariants()
            .Where(v => v.VariantCopies.Where(vl => vl.SerialNo.Equals(serialNo)).Any())
            .Select(v => v.Book).SingleOrDefault();
            if(book == null) return Json(new {successful = false, message= $"No book with {serialNo} was found"});
            return Json(new {successful= true, book = book});
        }
        private async Task PopulateDropdowns(BookCirculationListingViewModel bookCirculationListing) {
            bookCirculationListing.Statuses = await _context.CheckOutStatuses.OrderBy(t => t.Id).ToListAsync();
            bookCirculationListing.Days = termService.GetTermsBySet("book-days-allowed").OrderBy(t => t.Name).MapToSelectList().ToList();
        }
        private void PopulateDropdowns(ReserveBookFormViewModel model) {
            model.Days = termService.GetTermsBySet("book-days-allowed").OrderBy(t => t.Name).MapToSelectList().ToList();
            model.Locations = termService.GetTermsBySet("book-location").OrderBy(t => t.Name).MapToSelectList().ToList();
            model.Reasons = termService.GetTermsBySet("book-reservation-reason").OrderBy(t => t.Name).MapToSelectList().ToList();            

        }



    }
}