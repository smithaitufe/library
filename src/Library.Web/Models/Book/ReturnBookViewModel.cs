using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// using System.ComponentModel;
// using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.BookViewModels
{
    public class ReturnBookViewModel {
        public IList<CheckedBookViewModel> CheckOutBooks  { get; set; }
        [Display(Name="Borrowed Books")]
        public int SelectedCheckOutId { get; set;}
    }
}