using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Models.DashboardViewModels
{
    public class BookLocationStatisticsViewModel 
    {
        public BookLocationStatisticsViewModel() {
            Defaulters = new List<CheckOut>();
        }
        public int TotalBooksOnShelf { get; set; }
        public int TotalBooksOffShelf { get; set; }
        public int PendingReturnedBooks { get; set; }
        public int PendingCheckoutBooks { get; set; }
        public int ApprovedCheckoutBooks { get; set; }
        public int ConfirmedReturnedBooks { get; set; }
        public IList<CheckOut> Defaulters { get; set; }
    }
}