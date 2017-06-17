using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Code;
using Library.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Models.BookViewModels
{
    public class BookListViewModel
    {
        public int LocationId { get; set; }
        public ICollection<SelectListItem> Locations { get; set; }
        public SearchBookOptions SearchOptions { get; set; }
        public IEnumerable<DropdownTuple> BookFormats { get; set; }        
        public SortFilterPageOptions SortFilterPageData { get; set;}
        public IList<BookViewModel> Books { get; set; }
        public BookListViewModel(IList<BookViewModel> books, SearchBookOptions searchOptions, SortFilterPageOptions sortFilterPageData) {
            Books = books;
            SortFilterPageData = sortFilterPageData;
            SearchOptions = searchOptions;
        }



    }
}