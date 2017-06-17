using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Models.BookViewModels {
    public class BookCatalogueViewModel
    {
        public int CategoryId { get; set; }
        public int GenreId { get; set; }        
        public ICollection<SelectListItem> Categories { get; set; }= new HashSet<SelectListItem>();
        public ICollection<SelectListItem> Genres { get; set; }= new HashSet<SelectListItem>();
        public ICollection<BookViewModel> Books { get; set; } = new HashSet<BookViewModel>();
        
    }
}