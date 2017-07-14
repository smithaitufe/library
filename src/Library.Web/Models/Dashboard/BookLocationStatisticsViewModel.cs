namespace Library.Web.Models.DashboardViewModels
{
    public class BookLocationStatisticsViewModel 
    {
        public int TotalBooksOnShelf { get; set; }
        public int TotalBooksCheckedOut { get; set; }
        public int TotalBooksReturned { get; set; }
        public int TotalBooksOffShelf { get; set; }
        public int TotalBooks { get; set; }
    }
}