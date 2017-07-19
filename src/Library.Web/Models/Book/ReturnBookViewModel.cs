using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Library.Web.Models.BookViewModels
{
    public class ReturnBookViewModel {
        public IList<CheckoutViewModel> Checkouts  { get; set; }
        [Display(Name="Borrowed Books")]
        public int SelectedCheckOutId { get; set;}
    }
}