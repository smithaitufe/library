using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Models.BookViewModels
{
    public class CheckoutListingViewModel
    {
        public IList<CheckoutViewModel> Checkouts { get; set; }
    }
}