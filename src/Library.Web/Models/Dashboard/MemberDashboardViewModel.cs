using System.Collections.Generic;
using Library.Core.Models;
using Library.Web.Models.BookViewModels;

namespace Library.Web.Models.DashboardViewModels
{
    public class MemberDashboardViewModel {
        public User User { get; set; }
        public int TotalMembers { get; set; } = 0;
        public int TotalBooksBorrowed { get; set; }
        public int TotalBooksReturned { get; set; }
        public int BookBalance => TotalBooksBorrowed - TotalBooksReturned;

        public ICollection<Post> Posts { get; set; }
        public ICollection<Announcement> Announcements { get; set; }
        public IList<CheckOut> ExpireSoonCheckouts { get; set; }
        public IList<ExpiredCheckoutViewModel> ExpiredCheckouts { get; set; }
    }

}