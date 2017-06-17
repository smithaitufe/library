using System.Collections.Generic;
using Library.Code;
using Library.Core.Models;

namespace Library.Models.BookViewModels
{
    public class BookListingViewModel
    {
        
        public IEnumerable<DropdownTuple> BookFormats { get; set; } 
        public SearchBookOptions SearchOptions { get; set; }       
        public SortFilterPageOptions SortFilterPageData { get; set;}
        public ICollection<Book> Books { get; set; }

        public BookListingViewModel()
        {

        }
        public BookListingViewModel(IList<Book> books, SearchBookOptions searchOptions, SortFilterPageOptions sortFilterPageData)
        {
            
            Books = books;
            SortFilterPageData = sortFilterPageData;
            SearchOptions = searchOptions;
        
        }
    }
}