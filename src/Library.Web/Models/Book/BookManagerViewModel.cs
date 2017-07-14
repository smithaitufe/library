using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.BookViewModels
{
    public class BookManagerViewModel
    {
        public string SearchWords { get; set; }
        [DisplayName("Book Format")]
        public int BookFormatId { get; set; }
        public ICollection<SelectListItem> BookFormats { get; set; } = new List<SelectListItem>();
        public int GenreId { get; set; }
        public ICollection<SelectListItem> Genres { get; set; }
        public ICollection<SelectListItem> BookProperties { get; set; }
        public ICollection<BookViewModel> Books { get; set; }
        public ICollection<BookViewModel> BorrowedBooks { get; set; }

    }
}