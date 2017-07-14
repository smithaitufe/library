using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.BookViewModels
{
    public class SearchBookViewModel
    {
        public string Phrase { get; set; }
        public int GenreId { get; set; }
        public ICollection<BookViewModel> Books { get; set; }
        public int BookFormatId { get; set; }
        public ICollection<SelectListItem> BookFormats { get; set; } = new List<SelectListItem>();
        public ICollection<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public ICollection<SelectListItem> BookProperties { get; set; } = new List<SelectListItem>();

    }
}