using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Core.Models;
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

            var booksBorrowed = await _context.CheckOuts
            .Where(c => c.CheckOutStates.OrderByDescending(cs=>cs.Id)
                .Take(1)
                .Where(cs=>cs.Status.Name.ToLower().Equals("borrow approved"))
            .Any()).CountAsync();
            
            var booksReturned = await _context.CheckOuts.Where(c => 
                    c.CheckOutStates
                .Take(1)
                .Where(cs=>cs.Status.Name.ToLower().Equals("return initiated")).Any()
                ).CountAsync();

            await PopulateLocationDropdown();

            

            //model.TotalPendingUserRegistrations = _context.Users.Where(u => u.Approved == false && u.Roles.Where(r=>r.Role.Name.ToLower().Equals("member")).Any()).Count();
            //model.TotalUsers = _context.Users.Where(u => u.Approved == true && u.Roles.Where(r=>r.Role.Name.ToLower().Equals("member")).Any()).Count();
            return View(model);
        }


        public async Task<IActionResult> GetBookSummary(int locationId, Nullable<DateTime> date)
        {

            
            
            var books = _context.Variants.Where(v => v.VariantCopies.Where(vl=>vl.LocationId == locationId).Any());

            var totalBooksOnShelfQuery = books.Where(b => b.VariantCopies.Where(vl=>vl.Availability.Name.Equals("On Shelf")).Any());
            var totalPendingCheckoutBooksQuery = books.Where(b => b.CheckOuts.Where(c=> c.CheckOutStates.OrderByDescending(cs=>cs.Id).Take(1).Where(cs=>cs.Status.Name.ToLower().Equals("borrow initiated")).Any()).Any());
            var totalPendingReturnedCheckoutBooksQuery = books.Where(b => b.CheckOuts.Where(c=> c.CheckOutStates.OrderByDescending(cs=>cs.Id).Take(1).Where(cs=>cs.Status.Name.ToLower().Equals("return initiated")).Any()).Any());
            var approvedCheckoutBooks = await books.Where(b => b.CheckOuts.Where(c=> c.CheckOutStates.OrderByDescending(cs=>cs.Id).Take(1).Where(cs=>cs.Status.Name.ToLower().Equals("borrow approved")).Any()).Any()).CountAsync();
            var confirmedReturnedBooks = await books.Where(b => b.CheckOuts.Where(c=> c.CheckOutStates.OrderByDescending(cs=>cs.Id).Take(1).Where(cs=>cs.Status.Name.ToLower().Equals("return confirmed")).Any()).Any()).CountAsync();

            if(date.HasValue)
            {
                DateTime.TryParse(date.Value.ToString(), out var result);
                totalBooksOnShelfQuery = totalBooksOnShelfQuery.Where(b => b.VariantCopies.Where(vl=>vl.InsertedAt <= result).Any());
            }
            var defaulters = new List<CheckOut>();

            var checkouts = await books.Where(v => v.VariantCopies.Where(vc => vc.Out == true).Any()).SelectMany(v=>v.CheckOuts).ToListAsync();
            var len = checkouts.Count();
            for(var i = 0;  i < len; i++)
            {
                var checkout = checkouts[i];
                var approveState = checkout.CheckOutStates.Where(cs=>cs.Status.Name.ToLower().Equals("borrow approved")).SingleOrDefault();
                var dateApproved = approveState.InsertedAt;
                var numberOfDaysApproved = Convert.ToInt32(checkout.ApprovedDays.Name);
                var expectedReturnDate = dateApproved.AddDays(numberOfDaysApproved);

                var returned = checkout.CheckOutStates.Where(cs=>cs.Status.Name.ToLower().Equals("return confirmed")).SingleOrDefault();
                if(returned == null)
                {
                    var days = (DateTime.Now - expectedReturnDate).Days;
                    if(days > 0){ //Date has passed
                        defaulters.Add(checkout);
                    }
                }


            }
            
            var totalBooksOnShelf = totalBooksOnShelfQuery.Count();
            var totalPendingCheckoutBooks = totalPendingCheckoutBooksQuery.Count();
            var pendingReturnedCheckoutBooks = totalPendingReturnedCheckoutBooksQuery.Count();
            var bookStatistics = new BookLocationStatisticsViewModel(){ 
                TotalBooksOnShelf = totalBooksOnShelf,                
                PendingCheckoutBooks = totalPendingCheckoutBooks,
                PendingReturnedBooks = pendingReturnedCheckoutBooks,
                ApprovedCheckoutBooks = approvedCheckoutBooks,
                ConfirmedReturnedBooks = confirmedReturnedBooks,
                Defaulters = defaulters
            };

            

            return PartialView("_LocationStatisticsPartial", bookStatistics);
        }


        private async Task PopulateLocationDropdown()
        {
            var locations = await _context.Locations.OrderBy(l=>l.Name).ToListAsync();
            ViewBag.Locations = locations;
        }
    }
}