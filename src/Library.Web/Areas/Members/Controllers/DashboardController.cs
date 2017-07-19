using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Library.Core.Models;
using Library.Repo;
using Library.Web.Code;
using Library.Web.Models.BookViewModels;
using Library.Web.Models.DashboardViewModels;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Areas.Members.Controllers
{
    [Area(SiteAreas.Members)]
    [Authorize(Policy="MemberOnly")]
    public class DashboardController: Controller
    {
        LibraryDbContext _context;
        private long userId;
        public DashboardController(LibraryDbContext context) {
            _context = context;
        }
        public async Task<IActionResult> Index() {
            userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var statistics = new MemberDashboardViewModel();
            
            var posts = _context.Posts.OrderByDescending(p=>p.Id).Take(10).ToList();
            statistics.TotalBooksBorrowed = await CheckOutQuery("Borrow Approved").CountAsync();            
            statistics.TotalBooksReturned = await CheckOutQuery("Return Confirmed").CountAsync();
            statistics.Posts = posts;
            statistics.User  = await _context.Users.FindAsync(userId);    

            var expireSoonCheckouts = new List<CheckOut>();
            var expiredCheckouts = new List<ExpiredCheckoutViewModel>();


            var checkouts = _context.CheckOuts
            .Include(c=>c.Variant).ThenInclude(v=>v.Book)
            .Include(c=>c.Variant).ThenInclude(v=>v.Format)
            .Include(c=>c.Variant).ThenInclude(v=>v.Fine)
            .Include(c=>c.ApprovedDays)
            .Include(c => c.RequestedDays)
            .Include(c=>c.CheckOutStates).ThenInclude(cs=>cs.Status)
            .Include(c=>c.VariantCopy)
            .Where(c=>c.PatronId == userId && c.VariantCopy.Out == true)
            .ToList();

            var len = checkouts.Count();
            for(var i = 0;  i < len; i++)
            {
                var checkout = checkouts[i];
                var approveState = checkout.CheckOutStates.Where(cs=>cs.Status.Name.ToLower().Equals("borrow approved")).SingleOrDefault();
                var dateApproved = approveState.InsertedAt;
                var numberOfDaysApproved = Convert.ToInt32(checkout.ApprovedDays.Name);
                var returnDate = dateApproved.AddDays(numberOfDaysApproved);

                var returned = checkout.CheckOutStates.OrderByDescending(cs=>cs.Id).Take(1).Where(cs=>cs.Status.Name.ToLower().Equals("return confirmed")).SingleOrDefault();
                if(returned == null)
                {
                    var currentDate = DateTime.Now;
                    var days = (currentDate - returnDate).Days;
                    if(days >= 0 && days < 3)
                    { 
                        expireSoonCheckouts.Add(checkout);
                    }
                    else if (days < 0)
                    {
                        var expiredCheckout  = new ExpiredCheckoutViewModel {
                            CheckOut = checkout,
                            Days = days,
                            Charge = decimal.Parse(checkout.Variant.Fine.Name) * days
                        };
                        expiredCheckouts.Add(expiredCheckout);
                    }
                }


            }
            statistics.ExpireSoonCheckouts = expireSoonCheckouts;
            statistics.ExpiredCheckouts = expiredCheckouts;

            //statistics.Announcements = announcementService.GetAll().ToList();
            return View(statistics);
        }
        private IQueryable<CheckOut> CheckOutQuery(string status = "Borrow Approved")
        {
            return _context.CheckOuts
            .Where(c=>c.PatronId == userId)
            .Where(c => c.CheckOutStates.OrderByDescending(co=>co.Id).Take(1).Where(co=>co.Status.Name.ToLower().Equals(status.ToLower())).Any());
            
        }
    }
}