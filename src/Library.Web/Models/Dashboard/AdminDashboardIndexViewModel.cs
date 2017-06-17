using System;
using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Models.DashboardViewModels
{
    public class AdminDashboardIndexViewModel {
        public int TotalRegistrations { get; set; } = 0;
        public int TotalMembers { get; set; } = 0;
        public int TotalBooksBorrowed { get; set; } = 0;
        public int TotalBooksReturned { get; set; } = 0;
        public DateTime CurrentDateTime { get; set; }= DateTime.Now;

        public ICollection<Post> Posts { get; set; }
        public ICollection<Announcement> Announcements { get; set; }
    }
}