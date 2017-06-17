using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Models.DashboardViewModels
{
    public class DashboardIndexViewModel {
        public int TotalMembers { get; set; } = 0;
        public int TotalBooksBorrowed { get; set; }
        public int TotalBooksReturned { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Announcement> Announcements { get; set; }
    }
}