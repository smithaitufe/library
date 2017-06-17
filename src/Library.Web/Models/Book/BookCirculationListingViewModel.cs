
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Core.Models;

namespace Library.Models.BookViewModels
{
    public class BookCirculationListingViewModel {
        [Display(Name="Status")]
        public int StatusId { get; set; }
        [Display(Name="Approved Days")]
        public int ApprovedDaysId { get; set; }
        public ICollection<CheckOutStatus> Statuses {get; set; } = new HashSet<CheckOutStatus>();
        public ICollection<SelectListItem> Days {get; set; } = new HashSet<SelectListItem>();
        public BookCirculationSearchViewModel SearchCirculationsOptions { get; set; } = new BookCirculationSearchViewModel();
        public IList<BookCirculationViewModel> Circulations { get; set; } = new List<BookCirculationViewModel>();

        public BookCirculationListingViewModel() {
            Statuses = new HashSet<CheckOutStatus>();
            SearchCirculationsOptions = new BookCirculationSearchViewModel();
            Circulations  = new List<BookCirculationViewModel>();
        }
    }
}