using System;
using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Models.DashboardViewModels
{
    public class AdminDashboardIndexViewModel {
        public int TotalPendingUserRegistrations { get; set; } = 0;
        public int TotalUsers { get; set; } = 0;
        public DateTime CurrentDateTime { get; set; }= DateTime.Now;

        public ICollection<CheckOut> CheckOuts { get; set; } //collection of over due borrowed books.

        public ICollection<Post> Posts { get; set; }
        public ICollection<Announcement> Announcements { get; set; }
    }
}