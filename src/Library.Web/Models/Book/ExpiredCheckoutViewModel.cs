using Library.Core.Models;

namespace Library.Web.Models.BookViewModels
{
    public class ExpiredCheckoutViewModel
    {
        public CheckOut CheckOut { get; set; }
        public int Days { get; set; }
        public decimal Charge { get; set; }
    }
}