using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Models.BookViewModels
{
    public class BookAuthorListingViewModel
    {
        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}